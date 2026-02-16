using BookBuddy.API.Models.Domain;
using Microsoft.EntityFrameworkCore;


namespace BookBuddy.API.Data  // this is nothing but the Organization-"Groups related classes together" 
                              //Matches your folder structure
                              //Prevents naming conflicts
{ 
        public class BookBuddyDbContext : DbContext
        {
            public BookBuddyDbContext(DbContextOptions<BookBuddyDbContext> options) : base(options)  // ← This passes options to parent DbContext
            {
            //the body can be empty -no other initialization needed.  
            }

        // below our DbSet prop goes 
        public DbSet<User> Userss { get; set; }  // Userss, Rentalss... are Prop names. in EF core naming convention we  widely use plural names as per our classes.
        public DbSet<Rentals> Rentalss { get; set; }
        public DbSet<Payments> Paymentss { get; set; }
        public DbSet<Book> Bookss { get; set; }
    }
}

