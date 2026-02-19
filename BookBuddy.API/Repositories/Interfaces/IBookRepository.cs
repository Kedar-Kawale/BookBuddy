using BookBuddy.API.Models.Domain;

namespace BookBuddy.API.Repositories.Interfaces

{
    public interface IBookRepository
    {
        // now the Business requirement is to show all 500 books catalog on home Page

        Task<IEnumerable<Book>> GetAllBookAsync(); //  this is interface definition
        // we used the IEnumerable<> here for enhancing scalability and which can improve the performance by Lazy loading.
        // that is when use sees the catlog it will not load all 500 book on the UI , it will load progressively. 


    }
}
