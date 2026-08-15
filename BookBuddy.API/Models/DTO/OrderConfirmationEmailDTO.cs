namespace BookBuddy.API.Models.DTO
{
    public class OrderConfirmationEmailDTO
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string BookTitle { get; set; }
        public Guid RentalId { get; set; }
        public string PaymentId { get; set; }
        public int RentalDays { get; set; }
        public decimal RentalPrice { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime ExpectedReturnDate { get; set; }

    }
}
