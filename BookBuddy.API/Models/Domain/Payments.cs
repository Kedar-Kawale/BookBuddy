namespace BookBuddy.API.Models.Domain
{
    public class Payments
    {
        public Guid PaymentId { get; set; }     // Guid as PK
        public Guid RentalId { get; set; }      // FK to Rentals
        public decimal AmountPaid { get; set; }
        public string Status { get; set; } = "Pending"; // Pending/Success/Failed
    }
}
