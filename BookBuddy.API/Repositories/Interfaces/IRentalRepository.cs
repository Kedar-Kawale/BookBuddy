using BookBuddy.API.Models.DTO;

namespace BookBuddy.API.Repositories.Interfaces
{
    public interface IRentalRepository
    {
        Task<RentBookResponseDTO> RentBookAsync(Guid bookId, string userId, RentBookRequestDTO request);
        //bookId comes from the route.
        //RentalDays comes from RentBookRequestDTO
        //UserId comes from the logged-in user (JWT). so this repository need all of three.

    }
}
