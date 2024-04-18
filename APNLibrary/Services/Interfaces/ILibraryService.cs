using APNLibrary.DTOs;
using APNLibrary.Models.Books;
using APNLibrary.Models.Orders;

namespace APNLibrary.Services.Interfaces
{
    public interface ILibraryService
    {
        /// <summary>
        /// Sends asynchronous GET request to the api. Throws an exception when the request failed or response status code wasn't 200.
        /// </summary>
        /// <param name="token">Auth token from the api</param>
        /// <returns>The task object representing asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">Thrown when response status code wasn't 200.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request failed.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the request failed.</exception>
        /// <exception cref="JsonException">Thrown when te response stream could not be parsed to desired object.</exception>
        Task<IEnumerable<Book>> GetBooksAsync(string token);

        /// <summary>
        /// Sends asynchronous GET request to the api. Throws an exception when the request failed or response status code wasn't 200.
        /// </summary>
        /// <param name="token">Auth token from the api</param>
        /// <returns>The task object representing asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">Thrown when response status code wasn't 200.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request failed.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the request failed.</exception>
        /// <exception cref="JsonException">Thrown when te response stream could not be parsed to desired object.</exception>
        Task<PagedList<Order>> GetOrdersAsync(string token, int pageSize = 25, int pageNumber = 1);

        /// <summary>
        /// Sends asynchronous POST request to the api. Throws an exception when the request failed or response status code wasn't 200.
        /// </summary>
        /// <param name="book">Book to be added</param>
        /// <param name="token">Auth token from the api</param>
        /// <returns>The task object representing asynchronous operation.</returns>
        /// <exception cref="HttpRequestException">Thrown when response status code wasn't 200.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the request failed.</exception>
        /// <exception cref="TaskCanceledException">Thrown when the request failed.</exception>
        Task AddBookAsync(Book book, string token);

        /// <summary>
        /// A method that mimics retrieving a token.
        /// </summary>
        /// <returns>A token from the api</returns>
        Task<string> GetTokenAsync();
    }
}
