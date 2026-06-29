
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.API.Models.Domain
{
    public class Payments
    {
        [Key]
        public Guid PaymentId { get; set; }     // Guid as PK
        public Guid RentalId { get; set; }      // FK to Rentals
        public decimal AmountPaid { get; set; }
        public string Status { get; set; } = "Pending"; // Pending -> Success -> Failed

        public string? GatewayOrderId { get; set; }  //Stores the Razorpay Order ID created before payment.
        public string? GatewayPaymentId { get; set; } //Stores the Razorpay Payment ID after successful payment.
        public DateTime? PaymentDate { get; set; } //Records when the payment was successfully completed.
    }
}
