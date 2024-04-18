using APNLibrary.Models.Books;
using APNLibrary.Services;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;
using RichardSzalay.MockHttp;
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
        public async Task GetBooksAsync_WhenValidToken_ReturnsExpectedBooks()
        {
            // Arrange
            var expectedBooks = new[] { new Book { Id = 1, Title = "Book 1" }, new Book { Id = 2, Title = "Book 2" } };
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
            Assert.Equal(expectedBooks.Length, result.Count());
        }
    }
}