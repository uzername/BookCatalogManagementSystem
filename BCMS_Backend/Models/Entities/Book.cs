using BCMS_Backend.Entities;
using System.ComponentModel;

namespace BCMS_Backend.Entities
{
    public class Book:BaseEntity
    {
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public int IdCategory { get; set; }
        // === IGNORED FIELDS ===
        // to optimize getting list of all books with their respectable categories
        // I put here also name of category and parent category too
        [Description("ignore")]
        public string CategoryName { get; set; }
        public string ParentCategoryName { get; set; }
    }
}
