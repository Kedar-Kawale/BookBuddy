using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace BookBuddy.API.Controllers
{

    [Route("api/books")]
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
            // call repository to get all books
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
        [Authorize(Roles = "Admin")]
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
            // call repository to create book
            var createdBook = await _bookRepository.CreateBookAsync(book);
            // now this "createdBook" contains all the values including the generated 'BookId' etc above.

            // Map Domain model to Response DTO, because we never return the Domain model directly to the User.
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
        // GET: https://localhost:7258/api/Book/by-name?name=MindSet
        [HttpGet]
        [Route("by-name")]
        public async Task<ActionResult<BookResponseDTO>> GetBookByName([FromQuery] string name)
        {

            // Validation
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("Book name is required.");
            }


            //Get the book from the repository
            var book = await _bookRepository.GetByNameAsync(name);

            // Check if book exists, if not return a 404 Not Found response.
            if (book is null)
            {
                return NotFound();
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
        [Authorize(Roles = "Admin")]
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

            // Map Domain model to Response DTO, because we never return the Domain model directly to the User.

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

        //DELETE : https://localhost:7258/api/Book/{id}
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<ActionResult> DeleteBookById([FromRoute] Guid id)
        {
            // call repository
            var deletedBook = await _bookRepository.DeleteBookByIdAsync(id);

            // if null return NotFound()

            if (deletedBook == null)
            {
                return NotFound();  // if the book with the given id is not found, return a 404 Not Found response.
            }

            // Map Domain Model to Response DTO

            var response = new BookResponseDTO
            {
                BookId = deletedBook.BookId,
                Title = deletedBook.Title,
                Author = deletedBook.Author,
                Category = deletedBook.Category,
                ISBN = deletedBook.ISBN,
                Price = deletedBook.Price,
                TotalCopies = deletedBook.TotalCopies,
                AvailableCopies = deletedBook.AvailableCopies,
                PublishedAt = deletedBook.PublishedAt
            };
            // return your deleted book as result
            return Ok(response);
        }

        //=========================================================================================================

        //DELETE : https://localhost:7258/api/Book?name={name}
        [Authorize(Roles = "Admin")]
        [HttpDelete("by-name")]
        public async Task<ActionResult> DeleteBookByName([FromQuery] string name)
        {
            // call repository
            var deletedBook = await _bookRepository.DeleteBookByNameAsync(name);
            // if null return NotFound()
            if (deletedBook == null)
            {
                return NotFound();  // if the book with the given name is not found, return a 404 Not Found response.
            }
            // Map Domain Model to Response DTO
            var response = new BookResponseDTO
            {
                BookId = deletedBook.BookId,
                Title = deletedBook.Title,
                Author = deletedBook.Author,
                Category = deletedBook.Category,
                ISBN = deletedBook.ISBN,
                Price = deletedBook.Price,
                TotalCopies = deletedBook.TotalCopies,
                AvailableCopies = deletedBook.AvailableCopies,
                PublishedAt = deletedBook.PublishedAt
            };
            // return your deleted book as result
            return Ok(response);
        }

        //=========================================================================================================

        // GET: https://localhost:7258/api/books?page=1&pageSize=20
        [HttpGet("Catalog")]
        public async Task<IActionResult> GetBooks([FromQuery] BookCatalogRequestDTO request)
        {
            //Call the repository to get the books based on the request parameters
            var books = await _bookRepository.GetBooksAsync(request);

            //Map Domain Models to DTOs
            var response = books.Select(book => new BookCatalogResponseDTO
            {
                BookId = book.BookId,
                Title = book.Title,
                Author = book.Author,
                Category = book.Category,
                Price = book.Price,
                AvailableCopies = book.AvailableCopies
            });


            return Ok(response);
        }
    }
}




