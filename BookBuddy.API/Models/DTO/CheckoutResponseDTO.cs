namespace BookBuddy.API.Models.DTO
{
    public class CheckoutResponseDTO
    {

        public Guid PaymentId { get; set; }
        public Guid RentalId { get; set; }
        public string RazorpayOrderId { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Currency { get; set; } = "INR";
        public string Status { get; set; } = string.Empty;

    }
}
