using APNLibrary.DTOs;
using APNLibrary.Models.Books;
using APNLibrary.Models.Orders;

namespace APNLibrary.Services.Interfaces
{
    public interface ILibraryService
    {
        Task<IEnumerable<Book>> GetBooksAsync(string token);
        Task<PagedList<Order>> GetOrdersAsync(string token, int pageSize = 25, int pageNumber = 1);
        Task AddBookAsync(Book book, string token);
        Task<string> GetTokenAsync();
    }
}
