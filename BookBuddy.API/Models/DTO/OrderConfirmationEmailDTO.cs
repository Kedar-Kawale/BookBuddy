namespace BookBuddy.API.Models.DTO
{
    public class OrderConfirmationEmailDTO
    {
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string BookTitle { get; set; }
        public Guid RentalId { get; set; } // The unique identifier for the rental transaction
        public string PaymentId { get; set; } // The unique identifier for the payment transaction
        public int RentalDays { get; set; } // The number of days the book is rented for
        public decimal RentalPrice { get; set; } // The total price for the rental period
        public DateTime RentalDate { get; set; }  // The date when the rental was confirmed (i.e., when the payment was successful)
        public DateTime ExpectedReturnDate { get; set; } // The expected date when the book should be returned (i.e., RentalDate + RentalDays)

    }
}
