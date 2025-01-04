namespace BCMS_Backend.Helpers
{
    /// <summary>
    /// feels like a wrapper over some sort of list.
    /// Snatched from here:  https://dev.to/drsimplegraffiti/pagination-in-net-api-4opp
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;

        public PaginatedList(List<T> items, int pageIndex, int totalPages)
        {
            Items = items;
            PageIndex = pageIndex;
            TotalPages = totalPages;
        }
    }
}
