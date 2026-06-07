using BookBuddy.API.Models.Domain;

namespace BookBuddy.API.Repositories.Interfaces
{
    public interface IBookRepository
    {
        // Business requirement: is to show all 500 books catalog on home Page
        Task<IEnumerable<Book>> GetAllBookAsync(); //  this is interface definition ,and this GET method has not paramenter. i.e NO input needed
                                                   // we used the IEnumerable<> here for enhancing scalability and which can improve the performance by Lazy loading.
                                                   // that is when use sees the catlog it will not load all 500 book on the UI , it will load progressively. 

        //========================================================================================================================

        // Business Requirment: we as a admin need to create New books into our application.
        Task<Book?> CreateBookAsync(Book book);  // this method has paramenter , needs book details 

        //========================================================================================================================

        // Business requirement : user should be able to find a book by its Id. 
        Task<Book?> GetByIdAsync(Guid id);  // this method has paramenter , needs book id to find the book details.


        //========================================================================================================================

        // Business requirement : user shoud be able to update the book details by its Id.
        Task<Book?> UpdateBookAsync(Guid id, Book book);  // this method has 2 paramenter , needs book id to find the book details and also needs the 'updated book details' to update the book information in the database.


    }


}
    