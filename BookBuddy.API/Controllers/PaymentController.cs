using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace BookBuddy.API.Controllers

{
    [Route("api/payments")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        public PaymentController(IPaymentRepository paymentRepository)
        {
            this._paymentRepository = paymentRepository;
        }
        //----------constructor + DI done above ----------

        //---------methods and their routes paths below ---------
        [HttpPost("checkout/{rentalId:guid}")]
        public async Task<ActionResult> CheckOut(Guid rentalId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            CheckoutResponseDTO response = await _paymentRepository.CheckOutAsync(rentalId, userId);

            return Ok(response);
        }


        //===============================================================================================================

        [HttpPost("verify")]
        public async Task<ActionResult> VerifyPayment([FromBody] VerifyPaymentRequestDTO request)
        {
            await _paymentRepository.VerifyPaymentAsync(request);

            return Ok(new
            {
                Message = "Payment successful. Your rental has been confirmed.",
                PaymentId = request.RazorpayPaymentId
            });
        }
    }
}
