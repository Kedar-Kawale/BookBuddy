namespace BookBuddy.API.Models.DTO
{
    public class CreateBookRequestDTO
    {
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public string Category { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public decimal Price { get; set; }
        public int TotalCopies { get; set; }
        public DateTime PublishedAt { get; set; }

        // No internal fields like Books Domain Models
    }
}
