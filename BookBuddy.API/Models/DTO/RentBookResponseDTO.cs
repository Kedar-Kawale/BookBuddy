namespace BookBuddy.API.Models.DTO
{
    public class RentBookResponseDTO
    {
        public Guid RentalId { get; set; }

        public Guid BookId { get; set; }

        public int RentalDays { get; set; }

        public decimal RentalPrice { get; set; }

        public DateTime? ExpectedReturnDate { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

    }
}
