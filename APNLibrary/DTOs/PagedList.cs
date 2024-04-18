namespace APNLibrary.DTOs
{
    public class PagedList<T>
    {
        private PagedList(IEnumerable<T> items, int page, int pageSize, int totalCount)
        {
            Items = items;
            Page = page;
            PageSize = pageSize;
            TotalCount = totalCount;
        }

        public IEnumerable<T> Items { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
        public int TotalPages => Convert.ToInt32(Math.Ceiling((double)TotalCount / PageSize));

        internal static PagedList<T> Create(IEnumerable<T> collection, int pageSize, int pageNumber)
        {
            var totalCount = collection.Count();
            var items = collection.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, pageNumber, pageSize, totalCount);
        }
    }
}
