using BookBuddy.API.Models.Domain;
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
        public async Task<ActionResult<IEnumerable<Book>>> GetAllBook()
        {
             var books = await _bookRepository.GetAllBookAsync();

            return Ok(books);
        }

    }
}
