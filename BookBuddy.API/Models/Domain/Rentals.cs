using BookBuddy.API.Constants;
using System.ComponentModel.DataAnnotations;

namespace BookBuddy.API.Models.Domain
{
    public class Rentals
    {
        [Key]
        public Guid RentalId { get; set; }    //Guid as PK
        public string UserId { get; set; } = string.Empty;     //FK to User
        public Guid BookId { get; set; }   // FK to Book
        public DateTime? StartDate { get; set; }  // Rental starts when the book is delivered to the user
        public DateTime? EndDate { get; set; }  //Due Date
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;  //Used for request expiry.
        public string Status { get; set; } = RentalStatuses.Requested;  // Requested -> Paid -> Packed -> Shipped -> Delivered -> Returned or
                                                                        // Requested -> Expired
        public int RentalDays { get; set; }
        public decimal RentalPrice { get; set; }

    }
}
