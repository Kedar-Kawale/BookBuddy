using System.ComponentModel.DataAnnotations;

namespace BookBuddy.API.Models.Domain
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }          //PK as Guid
        public String Title { get; set; } = null!;
        public String Author { get; set; } = null!;
        public String Category { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public decimal Price { get; set; }
        public int TotalCopies { get; set; }
        public int AvailableCopies { get; set; }
        public DateTime PublishedAt { get; set; }
        public int Popularity { get; set; } = 0;
        public bool IsActive { get; set; } = true;
        public DateTime BookAddedAt { get; set; } = DateTime.UtcNow;

    }
}
