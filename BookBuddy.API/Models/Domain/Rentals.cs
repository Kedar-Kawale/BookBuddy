namespace BookBuddy.API.Models.Domain
{
    public class Rentals
    {
        public Guid RentalId { get; set; }    //Guid as PK
        public Guid UserId { get; set; }     //FK to User
        public Guid BookId { get; set; }   // FK to Book
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }  //Due Date
        public string Status { get; set; } = "Requested";  // Requested/Paid/Shipped/Delivered/Returned
    }
}
