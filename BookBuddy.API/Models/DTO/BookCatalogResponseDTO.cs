namespace BookBuddy.API.Models.DTO
{
    public class BookCatalogResponseDTO
    {
        public Guid BookId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int AvailableCopies { get; set; }


    }
}
