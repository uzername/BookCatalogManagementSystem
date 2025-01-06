using BCMS_Backend.Entities;
using CsvHelper.Configuration.Attributes;
using System.ComponentModel;

namespace BCMS_Backend.Entities
{
    // https://joshclose.github.io/CsvHelper/examples/configuration/attributes/
    public class Book:BaseEntity
    {
        [Index(1)]
        public string BookTitle { get; set; }
        [Index(0)]
        public string BookAuthor { get; set; }
        [Ignore]
        public int IdCategory { get; set; }
        // === IGNORED FIELDS ===
        // to optimize getting list of all books with their respectable categories
        // I put here also name of category and parent category too
        [Description("ignore")]
        [Index(2)]
        public string CategoryName { get; set; }
        [Description("ignore")]
        [Index(3)]
        public string ParentCategoryName { get; set; }
    }
}
