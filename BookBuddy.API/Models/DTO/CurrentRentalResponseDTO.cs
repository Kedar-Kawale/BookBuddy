namespace BookBuddy.API.Models.DTO
{
    public class CurrentRentalResponseDTO
    {
        public Guid RentalId { get; set; }

        public Guid BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public int RentalDays { get; set; }

        public decimal RentalPrice { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? ExpectedReturnDate { get; set; }

        public string RentalStatus { get; set; } = string.Empty;

        public string DueStatus { get; set; } = string.Empty;
    }
}

