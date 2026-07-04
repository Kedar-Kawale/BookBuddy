using BookBuddy.API.Configurations;
using BookBuddy.API.Constants;
using BookBuddy.API.Data;
using BookBuddy.API.Exceptions;
using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Razorpay.Api;

namespace BookBuddy.API.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BookBuddyDbContext _dbContext;
        private readonly RazorpaySettings _razorpaySettings;
        private readonly RazorpayClient _razorpayClient;

        public PaymentRepository(BookBuddyDbContext dbContext, IOptions<RazorpaySettings> razorpaySettings)
        {
            this._dbContext = dbContext;
            this._razorpaySettings = razorpaySettings.Value;
        }

        //-------------------constructor + DI above -----------------------------

        //------------------ Implemented methods below --------------------------

        public async Task<CheckoutResponseDTO> CheckOutAsync(Guid rentalId, string userId)
        {
            //validate the rentalId and userId, check if the rental exists and belongs to the user

            var rental = await _dbContext.Rentalss.FirstOrDefaultAsync(r => r.RentalId == rentalId);

            if (rental == null)
            {
                throw new NotFoundException("Rental not found.");
            }

            //validate Rental ownership, that is the rental belongs to the user
            if (rental.UserId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to checkout this rental.");
            }

            //validate the rental status, it should be "Pending" for checkout which means the rental is requested but not yet paid for.
            //If the rental is already paid, packed, shipped, delivered, or returned, it cannot be checked out again.
            if (rental.Status != RentalStatuses.Requested)
            {
                throw new ConflictException("Only requested rentals can proceed to checkout.");
            }

            //Smart duplicate Payments handling  for the same rental, check if a payment already exists for this rental.
            var existingPayment = await _dbContext.Paymentss.FirstOrDefaultAsync(p => p.RentalId == rentalId);

            if (existingPayment != null)
            {
                //payment already completed
                if (existingPayment.Status == PaymentStatuses.Success)
                {
                    throw new ConflictException("This rental has already been paid.");
                }

                // Payment is still pending
                if (existingPayment.Status == PaymentStatuses.Pending)
                {
                    throw new ConflictException("A payment is already pending for this rental.");
                }

                // Previous payment failed
                if (existingPayment.Status == PaymentStatuses.Failed)
                {
                    throw new ConflictException("The previous payment failed. Please contact support or retry later.");
                }
            }

            // Razorpay accepts amount in the smallest currency unit (Paise).
            int razorpayAmount = (int)(rental.RentalPrice * 100);

            // Create Razorpay Client
            var razorpayClient = new RazorpayClient(
                _razorpaySettings.KeyId,
                _razorpaySettings.KeySecret
                );

            // Prepare Razorpay Order Request
            var options = new Dictionary<string, object>
                {
                    {"amount", razorpayAmount },   //AMount in paisaa
                    {"currency", "INR" },           //Indian Rupees
                    {"receipt", rental.RentalId.ToString()  }   //Unique receipt id for this order
                };

            // Create the Razorpay Order
            Order order;

            try
            {
                order = razorpayClient.Order.Create(options);
            }
            catch (Exception)
            {
                throw new ConflictException("Unable to create the payment order. Please try again later.");
            }

            //Create Payment record in the database
            var payment = new Payments
            {
                PaymentId = Guid.NewGuid(),
                RentalId = rental.RentalId,
                AmountPaid = rental.RentalPrice,
                Status = PaymentStatuses.Pending,
                GatewayOrderId = order["id"].ToString(),
                GatewayPaymentId = null,
                PaymentDate = null,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Paymentss.AddAsync(payment);
            await _dbContext.SaveChangesAsync();

            //Return CheckoutResponseDTO

            return new CheckoutResponseDTO
            {
                PaymentId = payment.PaymentId,
                RentalId = payment.RentalId,
                RazorpayOrderId = payment.GatewayOrderId!,
                Amount = payment.AmountPaid,
                Currency = "INR",
                Status = payment.Status
            };
        }

        //==========================================================================================================

        public async Task VerifyPaymentAsync(VerifyPaymentRequestDTO request)
        {

            //create the Razorpay client:
            var razorpayClient = new RazorpayClient(
                _razorpaySettings.KeyId,
                _razorpaySettings.KeySecret
                );

            // Prepare payment verification attributes

            var attributes = new Dictionary<string, string>
            {
                {"razorpay_order_id",  request.RazorpayOrderId },
                {"razorpay_payment_id", request.RazorpayPaymentId },
                {"razorpay_signature", request.RazorpaySignature }
            };

            //Verify the Razorpay Signature
            try
            {
                Utils.verifyPaymentSignature(attributes);
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Invalid Razorpay payment signature.");
            }

            //Find the payment record in the database using the Razorpay Order ID
            var payment = await _dbContext.Paymentss.FirstOrDefaultAsync(p => p.GatewayOrderId == request.RazorpayOrderId);

            if (payment == null)
            {
                throw new NotFoundException("Payment record not found.");
            }

            //Update the payment record with the Razorpay Payment ID and mark it as successful
            payment.Status = PaymentStatuses.Success;
            payment.GatewayPaymentId = request.RazorpayPaymentId;
            payment.PaymentDate = DateTime.UtcNow;


            // Find the associated rental
            var rental = await _dbContext.Rentalss.FirstOrDefaultAsync(r => r.RentalId == payment.RentalId);

            if (rental == null)
            {
                throw new NotFoundException("Rental record not found.");
            }

            // Activate the rental after successful payment
            rental.Status = RentalStatuses.Paid;

            await _dbContext.SaveChangesAsync();

            //Here nothing to return as our method is void. and repository ends here.
        }
    }
}
