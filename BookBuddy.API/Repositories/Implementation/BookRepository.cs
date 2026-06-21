using BookBuddy.API.Data;
using BookBuddy.API.Models.Domain;
using BookBuddy.API.Models.DTO;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BookBuddy.API.Repositories.Implementation
{
    public class BookRepository : IBookRepository
    {
        private readonly BookBuddyDbContext _dbContext;  //Encapsulation achieved
        public BookRepository(BookBuddyDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        //-------------------constructor + DI above -----------------------------

        //------------------ Implemented methods below --------------------------
        public async Task<IEnumerable<Book>> GetAllBookAsync()
        {
            var Result = await _dbContext.Bookss.AsNoTracking().ToListAsync();   //instead of ToList(), I used ToListAsync() -for    Non-blocking of I/o thread and it enhances "scalability"
                                                                                 // AsNoTracking()  - by this the EF core do not track the "change tracking" for this GET Api only. so the CPU overhead will reduce and memory usage less.
                                                                                 // upcoming tasks: 
                                                                                 // query the database
                                                                                 // filtering 
                                                                                 // sorting 
                                                                                 // pagination
            return (Result);
        }

        //========================================================================================
        public async Task<Book?> CreateBookAsync(Book brocode)

        {
            //add book to db
            // formula : _dbContext + collection/DbSet + YOUR_METHODS
            await _dbContext.Bookss.AddAsync(brocode);

            //save book to db
            await _dbContext.SaveChangesAsync();

            //returned saved book
            return brocode;
        }

        //========================================================================================
        public async Task<Book?> GetByIdAsync(Guid id)
        {

            var popat = await _dbContext.Bookss.FindAsync(id);

            return popat;         // no need of SaveChangesAsync() because we are just fetching data, not modifying it.
                                  // FindAsync() is used for fetching data by primary key, and it is more efficient than FirstOrDefaultAsync() when you are searching by primary key.
                                  // SaveChangesAsync() is used for INSERT/UPDATE/DELETE operations, and it is not needed for SELECT or Reading operations.
        }

        //========================================================================================
        public async Task<Book?> GetByNameAsync(string name)
        {
            var Chimni = await _dbContext.Bookss.FirstOrDefaultAsync(b => b.Title.Contains(name));

            return Chimni;

        }
        //========================================================================================
        public async Task<Book?> UpdateBookAsync(Guid id, Book book)
        {
            // first we need to find the book by its id, if not found return null, and then update will happen.

            var existingBook = await _dbContext.Bookss.FirstOrDefaultAsync(b => b.BookId == id);

            // now here two cases.
            // 1. if book is not found then we will return null.

            if (existingBook == null)
            {
                return null;
            }

            // 2. if book is found then we will update the book and save changes to db.

            existingBook.Title = book.Title;
            existingBook.Author = book.Author;
            existingBook.Category = book.Category;
            existingBook.ISBN = book.ISBN;
            existingBook.Price = book.Price;
            existingBook.TotalCopies = book.TotalCopies;
            existingBook.AvailableCopies = book.AvailableCopies;
            existingBook.PublishedAt = book.PublishedAt;
            existingBook.IsActive = book.IsActive;   // here ek ek ko chun chun ke update kiya and 


            await _dbContext.SaveChangesAsync();  // yaha sabkoo update kiya aur fir save kiya.
            return existingBook;
        }

        //========================================================================================
        public async Task<Book?> DeleteBookByIdAsync(Guid id)
        {
            // find the book by id 
            var book = await _dbContext.Bookss.FirstOrDefaultAsync(b => b.BookId == id);


            //If book is null → return null.

            if (book == null)
            {
                return null;
            }

            //if book foudn then Remove the book from DbContext

            _dbContext.Bookss.Remove(book);
            await _dbContext.SaveChangesAsync();

            // return the deleted book
            return book;
        }
        //========================================================================================
        public async Task<Book?> DeleteBookByNameAsync(string name)
        {
            // find a book by its name 
            var book = await _dbContext.Bookss.FirstOrDefaultAsync(b => b.Title == name);

            // if book not found then return null
            if (book == null)
            {
                return null;
            }

            // if book found then delete that book
            _dbContext.Bookss.Remove(book);
            await _dbContext.SaveChangesAsync();

            //return the deleted book
            return book;
        }

        //========================================================================================
        public async Task<IEnumerable<Book>> GetBooksAsync(BookCatalogRequestDTO request)
        {
            var books = _dbContext.Bookss.AsNoTracking().AsQueryable(); // as this is only readonly Query so using the AsNoTracking() for performance. 

            // Filtering Title
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                books = books.Where(x => x.Title.Contains(request.Title));
                // Filtering Author

            }
            if (!string.IsNullOrWhiteSpace(request.Author))
            {
                books = books.Where(x => x.Author.Contains(request.Author));
            }
            // Filtering Category
            if (!string.IsNullOrWhiteSpace(request.Category))
            {
                books = books.Where(x => x.Category.Contains(request.Category));
            }

            //Add pagination now 
            books = books
                .OrderBy(b => b.Title)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize);


            return await books.ToListAsync();
        }
    }
}
