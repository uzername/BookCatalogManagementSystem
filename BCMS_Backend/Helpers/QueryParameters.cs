namespace BCMS_Backend.Helpers
{
    /// <summary>
    /// Query parameters for BookController::GetBooksWithPaginationAndFiltering. Inspired by: 
    /// https://unitcoding.com/filtering-your-web-api-resources/
    /// </summary>
    public class QueryParameters
    {
       public int pageIndex { get; set; } 
       public int pageSize { get; set; }
        // === filtering options ===
        public string BookTitleFilter { get; set; }
        public string BookAuthorFilter { get; set; }
        public string CategoryFilter { get; set; }
    }
}
