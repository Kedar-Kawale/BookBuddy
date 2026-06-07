using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Implementation;
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

            //Map Domain Models to DTOs via Foreach loop
            var response = new List<BookResponseDTO>();  // created new empty List obj for DTOs, and stored it into 'response' variable.

            foreach (var book in books) // loop through eachb Domain models
            {
                response.Add(new BookResponseDTO     // create new DTO instance
                {
                    BookId = book.BookId,     //{Copy each property (Domain -->DTO) we are here mapping Domain models into DTO, from RHS to LHS.}
                    Title = book.Title,
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

        //=========================================================================================================
        //POST : https://localhost:7258/api/Book
        [HttpPost]
        public async Task<ActionResult<BookResponseDTO>> CreateBook([FromBody] CreateBookRequestDTO request)
        {
            // convert incoming DTO to domain model 
            var book = new Book
            {
                Title = request.Title,
                Author = request.Author,
                Category = request.Category,
                ISBN = request.ISBN,
                Price = request.Price,
                TotalCopies = request.TotalCopies,
                PublishedAt = request.PublishedAt,

                AvailableCopies = request.TotalCopies,  // now these below prop are not coming from the User, so we as an Admin assigning them with Users fileds
                Popularity = 0,                          // so whenever we received the Book from User , it automatically add these assigned filed with it.
                IsActive = true,
                BookAddedAt = DateTime.UtcNow
            };

            var createdBook = await _bookRepository.CreateBookAsync(book);
            // now this "createdBook" contains all the values including the generated 'BookId' etc above.

            // Map Domain model to Response DTO
            var response = new BookResponseDTO
            {
                BookId = createdBook.BookId,
                Title = createdBook.Title,
                Author = createdBook.Author,
                Category = createdBook.Category,
                ISBN = createdBook.ISBN,
                Price = createdBook.Price,
                TotalCopies = createdBook.TotalCopies,
                AvailableCopies = createdBook.AvailableCopies,
                PublishedAt = createdBook.PublishedAt
            };

            return Ok(response);
        }

        //=========================================================================================================

        // GET: https://localhost:7258/api/Book/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<ActionResult<BookResponseDTO>> GetBookById([FromRoute] Guid id)
        {
            //Get the Book from Repository
            var book = await _bookRepository.GetByIdAsync(id);

            if (book is null)
            {
                return NotFound();  // if the book with the given id is not found, return a 404 Not Found response.
            }

            //convert Domain model to DTO
            var response = new BookResponseDTO()
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Category = book.Category,
                ISBN = book.ISBN,
                Price = book.Price,
                TotalCopies = book.TotalCopies,
                AvailableCopies = book.AvailableCopies,
                PublishedAt = book.PublishedAt
            };
            return Ok(response);
        }

        //=========================================================================================================

        //PUT: https://localhost:7258/api/Book/{id}
        [HttpPut]
        [Route("{id:guid}")]

        public async Task<ActionResult> UpdateBook([FromRoute] Guid id, UpdateBookRequestDTO requestDTO)
        {
            // convert incoming request DTO to domain model

            var book = new Book
            {
                Title = requestDTO.Title,
                Author = requestDTO.Author,
                Category = requestDTO.Category,
                ISBN = requestDTO.ISBN,
                Price = requestDTO.Price,
                TotalCopies = requestDTO.TotalCopies,
                AvailableCopies = requestDTO.AvailableCopies,
                PublishedAt = requestDTO.PublishedAt,
                IsActive = requestDTO.IsActive
            };
            // call the repository to update the book
            var updatedBook = await _bookRepository.UpdateBookAsync(id, book);

            if (updatedBook == null)
            {
                return NotFound();  // if the book with the given id is not found, return a 404 Not Found response.
            }

            // Map Domain model to Response DTO

            var response = new BookResponseDTO
            {
                BookId = updatedBook.BookId,
                Title = updatedBook.Title,
                Author = updatedBook.Author,
                Category = updatedBook.Category,
                ISBN = updatedBook.ISBN,
                Price = updatedBook.Price,
                TotalCopies = updatedBook.TotalCopies,
                AvailableCopies = updatedBook.AvailableCopies,
                PublishedAt = updatedBook.PublishedAt

            };

            return Ok(response);
        }

        //=========================================================================================================


    }
}




