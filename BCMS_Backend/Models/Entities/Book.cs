using BCMS_Backend.Entities;

namespace BCMS_Backend.Entities
{
    public class Book:BaseEntity
    {
        public string BookName { get; set; }
        public string BookTitle { get; set; }
        public string BookAuthor { get; set; }
        public int IdCategory { get; set; }
    }
}
