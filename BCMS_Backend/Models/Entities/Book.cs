using BCMS_Backend.Entities;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using System.ComponentModel;
using System.Globalization;

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
    /// <summary>
    /// required for configuration of csv parser
    /// https://joshclose.github.io/CsvHelper/examples/configuration/class-maps/ignoring-properties/
    /// </summary>
    public sealed class BookMap : ClassMap<Book>
    {
        public BookMap()
        {
            AutoMap(CultureInfo.InvariantCulture);
            Map(m => m.Id).Ignore();
            Map(m=>m.IdCategory).Ignore();
        }
    }
}
