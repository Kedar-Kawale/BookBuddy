using BookBuddy.API.Models.DTO;

namespace BookBuddy.API.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<CheckoutResponseDTO> CheckOutAsync(Guid rentalId, string userId);  //These method parameters are coming from the Url,  and userId is coming from the JWT token.



        Task VerifyPaymentAsync(VerifyPaymentRequestDTO verifyPaymentRequestDTO);  //we are returning the 'Task' and Not 'Task<DTO>',  The purpose of this API is to perform an action, not to retrieve data.
        //if the verification succeeds: payment is updated, rental is updated , API return 200 OK.
        //if the verification fails :We throw an exception. payment is not updated, rental is not updated , API return 400 BAD request.





    }
}


