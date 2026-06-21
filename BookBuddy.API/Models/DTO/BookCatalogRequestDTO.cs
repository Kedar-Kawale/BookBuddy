namespace BookBuddy.API.Models.DTO
{
    public class BookCatalogRequestDTO
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Category { get; set; }
    }
}
