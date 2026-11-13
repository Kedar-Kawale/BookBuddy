using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookBuddy.API.Controllers
{
    [ApiController]
    [Route("api/rentals")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalsController(IRentalRepository rentalRepository)
        {
            this._rentalRepository = rentalRepository;
        }
        //----------constructor + DI done above ----------

        //---------methods and their routes paths below ---------

        //POST /api/rentals/{bookId}
        [HttpPost("{bookId}")]
        [Authorize]
        public async Task<ActionResult> RentBook([FromRoute] Guid bookId, [FromBody] RentBookRequestDTO request)
        {

            //Extract the logged in User first
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            // call the repository 
            var response = await _rentalRepository.RentBookAsync(bookId, userId, request);

            return Ok(response);
        }


        // GET /api/rentals/my-rentals
        [HttpGet("my-rentals")]
        [Authorize]
        public async Task<ActionResult<List<CurrentRentalResponseDTO>>> GetMyRentals()
        {
            // Extract the logged-in user's ID from JWT claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Retrieve the user's current rentals
            var rentals = await _rentalRepository.GetCurrentRentalsAsync(userId);

            // Map Domain Models to Response DTOs
            var response = rentals.Select(r => new CurrentRentalResponseDTO
            {
                RentalId = r.RentalId,
                BookId = r.BookId,
                BookTitle = r.Book?.Title ?? string.Empty,
                Author = r.Book?.Author ?? string.Empty,
                RentalDays = r.RentalDays,
                RentalPrice = r.RentalPrice,
                StartDate = r.StartDate,
                ExpectedReturnDate = r.EndDate,
                RentalStatus = r.Status,
                DueStatus = GetDueStatus(r.StartDate, r.EndDate)
            }).ToList();

            return Ok(response);
        }


        private static string GetDueStatus(DateTime? startDate, DateTime? expectedReturnDate)
        {
            // Rental period has not started yet
            if (startDate == null || expectedReturnDate == null)
            {
                return "NotStarted";
            }


            // Return deadline is today
            if (expectedReturnDate.Value.Date == DateTime.UtcNow.Date)
            {
                return "DueToday";
            }

            var remainingTime = expectedReturnDate.Value - DateTime.UtcNow;

            // Return deadline has passed
            if (remainingTime <= TimeSpan.Zero)
            {
                return "Overdue";
            }


            // Two days or less remaining
            if (remainingTime <= TimeSpan.FromDays(2))
            {
                return "DueSoon";
            }

            return "OnTrack";
        }


        [HttpGet("my-rental-history")]
        [Authorize]
        public async Task<IActionResult> GetRentalHistory()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var rentals = await _rentalRepository.GetRentalHistoryAsync(userId);


            var response = rentals.Select(
                  r => new CurrentRentalResponseDTO
                  {
                      RentalId = r.RentalId,
                      BookId = r.BookId,
                      BookTitle = r.Book?.Title ?? string.Empty,
                      Author = r.Book?.Author ?? string.Empty,
                      RentalDays = r.RentalDays,
                      RentalPrice = r.RentalPrice,
                      StartDate = r.StartDate,
                      ExpectedReturnDate = r.EndDate,
                      RentalStatus = r.Status,
                      DueStatus = string.Empty

                  }).ToList();

            return Ok(response);
        }


        // these many APIs are required in this controller :

        //GET /api/rentals/{rentalId -Get rental details 


        //below are the in future APIs 
        //PATCH  /api/rentals/{rentalId}/packed
        //PATCH  /api/rentals/{rentalId}/shipped
        //api/rentals/{rentalId}/delivered
        //PATCH  /api/rentals/{rentalId}/returned
    }
}
