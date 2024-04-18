using APNLibrary.DTOs;
using APNLibrary.Models.Books;
using APNLibrary.Models.Orders;
using APNLibrary.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Text.Json;

namespace APNLibrary.Services
{
    /// <summary>
    /// A library service that uses an external API.
    /// </summary>
    public class LibraryService : ILibraryService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public LibraryService(IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, string baseUrl)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri(baseUrl);
            _memoryCache = memoryCache;
        }

        /// <inheritdoc/>
        public async Task AddBookAsync(Book book, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var content = new StringContent(JsonSerializer.Serialize(book), System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("/api/books", content);
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Book>> GetBooksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/books");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            return await JsonSerializer.DeserializeAsync<IEnumerable<Book>>(responseStream, options)
                ?? throw new JsonException("Could not parse the response from api");
        }

        /// <inheritdoc/>
        public async Task<PagedList<Order>> GetOrdersAsync(string token, int pageSize = 25, int pageNumber = 1)
        {
            if (_memoryCache.TryGetValue("orders", out IEnumerable<Order>? cachedOrders) && cachedOrders is not null)
                return PagedList<Order>.Create(cachedOrders, pageSize, pageNumber);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("/api/orders");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var orders =  await JsonSerializer.DeserializeAsync<IEnumerable<Order>>(responseStream, options)
                ?? throw new JsonException("Could not parse the response from api");

            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(60));

            _memoryCache.Set("orders", orders, cacheEntryOptions);

            return PagedList<Order>.Create(orders, pageSize, pageNumber);
        }

        /// <inheritdoc/>
        public async Task<string> GetTokenAsync()
        {
            return await Task.FromResult("exampleToken");
        }
    }
}
