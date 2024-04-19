# APNLibrary ![NuGet Version](https://img.shields.io/nuget/v/APNLibrary)

A library service that uses an external API.

## Installation

Use the .NET CLI to install.

```bash
dotnet add package APNLibrary --version 1.0.1
```

## Usage

Register service in DI container. For .NET built in container use:
```csharp
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ILibraryService>(provider =>
{
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var memoryCache = provider.GetRequiredService<IMemoryCache>();
    var baseUrl = "<YOUR_API_URL>";

    return new LibraryService(httpClientFactory, memoryCache, baseUrl);
});
```

Then you can use provided methods in your services simply by dependency injection.

```csharp
public class TestService
{
    private readonly ILibraryService _libraryService;

    public TestService(ILibraryService libraryService)
    {
        _libraryService = libraryService;
    }

    public async Task<IEnumerable<Book>> GetBooks()
    {
        try
        {
            var token = await _libraryService.GetTokenAsync();
            return await _libraryService.GetBooksAsync(token);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
```
