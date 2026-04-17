using BookBuddy.API.Data;
using BookBuddy.API.Models.Domain;
using BookBuddy.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
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
          var Result = await _dbContext.Bookss.ToListAsync();   //instead of ToList(), I used ToListAsync() -for    Non-blocking of I/o thread and it enhances "scalability"
            // upcoming tasks: 
            // query the database
            // filtering 
            // sorting 
            // pagination
            return (Result);
        }


    //========================================================================================
        public async Task<Book> CreateBookAsync(Book book)
        {
            //add book to db
          await _dbContext.Bookss.AddAsync(book); 

            //save book to db
           await _dbContext.SaveChangesAsync();

            //returned saved book
            return book;    
        }




    }
}
