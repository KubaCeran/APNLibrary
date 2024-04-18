namespace APNLibrary.Models.Books
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public decimal Price { get; set; }
        public int Bookstand { get; set; }
        public int Shelf { get; set; }
        public IEnumerable<Author> Authors { get; set; } = new List<Author>();
    }
}
