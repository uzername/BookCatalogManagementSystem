
using System.ComponentModel;
using System.Globalization;

namespace BCMS_FrontendBlazor.Models
{
    // https://joshclose.github.io/CsvHelper/examples/configuration/attributes/
    public class Book
    {
        public int Id { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public int IdCategory { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }

        public override string ToString()
        {
            string titlePartial = String.IsNullOrEmpty(BookTitle) ? "..." : BookTitle;
            string authorPartial = String.IsNullOrEmpty(BookAuthor) ? "Anonymous" : BookAuthor;
            return $"{titlePartial} ({authorPartial})";
        }
    }

}
