using BookBuddy.API.Constants;
using BookBuddy.API.Data;
using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.API.Repositories.Implementation
{
    public class RentalRepository : IRentalRepository
    {
        private readonly BookBuddyDbContext _dbContext;    //encapsulation.
        private readonly IConfiguration _configuration;    // now repository has access to the configuration settings.

        public RentalRepository(BookBuddyDbContext dbContext, IConfiguration configuration)
        {
            this._dbContext = dbContext;
            this._configuration = configuration;
        }

        //-------------------constructor + DI above -----------------------------

        //------------------ Implemented methods below --------------------------

        public async Task<RentBookResponseDTO> RentBookAsync(Guid bookId, string userId, RentBookRequestDTO request)
        {
            // check if the book is available for rent
            var book = await _dbContext.Bookss.FirstOrDefaultAsync(b => b.BookId == bookId);

            // check if the book exists 
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            // validate if the book is available for rent
            if (!book.IsActive)
            {
                throw new InvalidOperationException("This book is currently inactive and cannot be rented.");
            }

            //validate if the book has available copies
            if (book.AvailableCopies <= 0)
            {
                throw new InvalidOperationException("This book is currently out of stock and cannot be rented.");
            }

            //validate the rental days
            var allowedRentalDays = new[] { 7, 15, 20, 30 };

            if (!allowedRentalDays.Contains(request.RentalDays))
            {
                throw new ArgumentException("Custom rental durations are not allowed at this time.");
            }

            //Prevent Duplicate Active Rentals
            //find  existing rental for the same book and user.

            var existingRental = await _dbContext.Rentalss.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId && r.Status == RentalStatuses.Requested);

            //check if exsistingRental is present or not.
            if (existingRental != null)
            {
                //Check Whether the Requested Rental Has Expired or not 
                var rentalExpiryHours = _configuration.GetValue<int>("RentalSettings:RentalExpiryHours");

                //checking if the rental has expired by comparing the created date with the current date and time.
                bool isExpired = existingRental.CreatedAt.AddHours(rentalExpiryHours) <= DateTime.UtcNow;

                if (!isExpired)
                {
                    // Default message when the user is continuing with the same rental.
                    string message = "You already have an active rental for this book. Continue with checkout.";

                    // Allow the user to change the rental duration before payment
                    if (existingRental.RentalDays != request.RentalDays)
                    {
                        decimal updatedRentalPrice = request.RentalDays switch
                        {
                            7 => 79,
                            15 => 149,
                            20 => 199,
                            30 => 279,
                            _ => throw new ArgumentException("Invalid rental duration.")
                        };

                        existingRental.RentalDays = request.RentalDays;
                        existingRental.RentalPrice = updatedRentalPrice;

                        await _dbContext.SaveChangesAsync();

                        message = "Rental request updated successfully. Proceed to checkout.";
                    }
                    // if not expired then Return this rental instead of creating a new one.
                    return new RentBookResponseDTO
                    {
                        RentalId = existingRental.RentalId,
                        BookId = existingRental.BookId,
                        RentalDays = existingRental.RentalDays,
                        RentalPrice = existingRental.RentalPrice,
                        ExpectedReturnDate = existingRental.EndDate,
                        Status = existingRental.Status,
                        Message = message
                    };
                }
                else
                {
                    existingRental.Status = RentalStatuses.Expired;
                    await _dbContext.SaveChangesAsync();
                }
            }

            // Execution reaches here only if NO existing rental is found or the existing rental has expired.
            // Now we can proceed to create a new rental. that is Happy path.

            //Now calculate Rental Price 
            decimal rentalPrice = request.RentalDays switch
            {
                7 => 79,
                15 => 149,
                20 => 199,
                30 => 279,
                _ => throw new ArgumentException("Invalid rental duration.")
            };

            //Create a new Rental Entity
            var rental = new Rentals
            {
                RentalId = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
                CreatedAt = DateTime.UtcNow,
                RentalDays = request.RentalDays,
                RentalPrice = rentalPrice,
                Status = RentalStatuses.Requested
            };

            await _dbContext.Rentalss.AddAsync(rental);
            await _dbContext.SaveChangesAsync();

            //Return the response DTO
            return new RentBookResponseDTO
            {
                RentalId = rental.RentalId,
                BookId = rental.BookId,
                RentalDays = rental.RentalDays,
                RentalPrice = rental.RentalPrice,
                ExpectedReturnDate = default,  //rental starts only after delivery ,therefore there is no expected return date yet.
                Status = rental.Status,
                Message = "Rental request created successfully. Proceed to checkout."
            };
        }
    }
}
