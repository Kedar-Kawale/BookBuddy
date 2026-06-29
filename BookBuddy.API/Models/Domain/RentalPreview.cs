namespace BookBuddy.API.Models.Domain
{
    public class RentalPreview
    {
        public Guid BookId { get; set; }

        public string Title { get; set; } = string.Empty;

        public int RentalDays { get; set; }

        public decimal RentalPrice { get; set; }

        public DateTime ExpectedReturnDate { get; set; }

        public string AvailabilityStatus { get; set; } = string.Empty;

    }
}
