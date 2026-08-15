using BookBuddy.API.Models.DTO;

namespace BookBuddy.API.Email
{
    public interface IEmailService
    {
        Task SendOrderConfirmationEmailAsync(OrderConfirmationEmailDTO orderConfirmationEmailDTO);
    }
}
