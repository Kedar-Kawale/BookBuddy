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








        // these many APIs are required in this controller :

        //GET /api/rentals/{rentalId -Get rental details 
        //GET /api/rentals/my-rentals -List Current User's rentals


        //below are the in future APIs 
        //PATCH  /api/rentals/{rentalId}/packed
        //PATCH  /api/rentals/{rentalId}/shipped
        //api/rentals/{rentalId}/delivered
        //PATCH  /api/rentals/{rentalId}/returned
    }
}
