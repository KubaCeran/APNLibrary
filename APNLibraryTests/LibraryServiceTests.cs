using APNLibrary.Models.Books;
using APNLibrary.Models.Orders;
using APNLibrary.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using RichardSzalay.MockHttp;
using System.Net;
using System.Text.Json;

namespace APNLibraryTests
{
    public class LibraryServiceTests
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMemoryCache _memoryCache;

        public LibraryServiceTests()
        {
            _httpClientFactory = Substitute.For<IHttpClientFactory>();
            _memoryCache = Substitute.For<IMemoryCache>();
        }

        [Fact]
        public async Task GetBooksAsync_ReturnsExpectedBooks()
        {
            // Arrange
            var expectedBooks = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1" },
                new Book { Id = 2, Title = "Book 2" }
            };
            var expectedBooksJson = JsonSerializer.Serialize(expectedBooks);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://example.com/api/books")
                .Respond("application/json", expectedBooksJson);

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act
            var result = await service.GetBooksAsync("valid_token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedBooks.Count(), result.Count());
        }

        [Fact]
        public async Task GetBooksAsync_WhenInvalidToken_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://example.com/api/books")
                .Respond(HttpStatusCode.Unauthorized);

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetBooksAsync("invalid_token"));
        }

        [Fact]
        public async Task GetOrdersAsync_ReturnsCorrectPagedOrders()
        {
            // Arrange
            var expectedOrders = new List<Order>
            {
                new Order { OrderId = Guid.NewGuid().ToString(), OrderLines = new List<OrderLine> { new OrderLine { BookId = 1, Quantity = 2 } } },
                new Order { OrderId = Guid.NewGuid().ToString(), OrderLines = new List<OrderLine> { new OrderLine { BookId = 3, Quantity = 1 } } }
            };
            var expectedOrdersJson = JsonSerializer.Serialize(expectedOrders);

            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://example.com/api/orders")
                .Respond("application/json", expectedOrdersJson);

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act
            var result = await service.GetOrdersAsync("valid_token");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedOrders.Count(), result.Items.Count());
        }

        [Fact]
        public async Task GetOrdersAsync_WhenInvalidToken_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Get, "https://example.com/api/orders")
                .Respond(HttpStatusCode.Unauthorized);

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.GetOrdersAsync("invalid_token"));
        }

        [Fact]
        public async Task AddBookAsync_WhenBookIsNull_ThrowsHttpRequestException()
        {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://example.com/api/books")
                .Respond(req => new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest));

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.AddBookAsync(null!, "valid_token"));
        }

        [Fact]
        public async Task AddBookAsync_WhenInvalidToken_ThrowsHttpRequestException()
        {
            // Arrange
            var book = new Book { Id = 1, Title = "Book 1" };
            var mockHttp = new MockHttpMessageHandler();
            mockHttp.When(HttpMethod.Post, "https://example.com/api/books")
                .Respond(HttpStatusCode.Unauthorized);

            var httpClient = new HttpClient(mockHttp);
            _httpClientFactory.CreateClient().Returns(httpClient);

            var service = new LibraryService(_httpClientFactory, _memoryCache, "https://example.com");

            // Act & Assert
            await Assert.ThrowsAsync<HttpRequestException>(() => service.AddBookAsync(book, "invalid_token"));
        }
    }
}