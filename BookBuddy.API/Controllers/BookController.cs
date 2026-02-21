using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BookBuddy.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
               this._bookRepository = bookRepository;
        }

        //----------constructor + DI done above ----------

        //---------methods and their routes paths below ---------

        
        // GET: https://localhost:7258/api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBook()   // <--here in the controller's methods we do not use the "Async" as the suffix to keep our controller clean and neat.
        {
            var books = await _bookRepository.GetAllBookAsync();

            //Map Domain Models to DTOs
           var response = new List<BookResponseDTO>();  // created new empty List obj for DTOs, and stored it into 'response' variable.

            foreach(var book in books) // loop through eachb Domain models
            {
                response.Add(new BookResponseDTO     // create new DTO instance
                {
                    BookId = book.BookId,     //{Copy each property (Domain -->DTO) we are here mapping Domain models into DTO, from RHS to LHS.}
                    Title  = book.Title,
                    Author = book.Author,
                    Category = book.Category,
                    ISBN = book.ISBN,
                    Price = book.Price,
                    TotalCopies = book.TotalCopies,
                    AvailableCopies = book.AvailableCopies,
                    PublishedAt = book.PublishedAt
                });                                 
            }
            return Ok(response);    // returning result 
        }
    }
}
