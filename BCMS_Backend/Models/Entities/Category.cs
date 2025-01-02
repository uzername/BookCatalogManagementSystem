namespace BCMS_Backend.Entities
{
    public class Category:BaseEntity
    {
        public String CategoryName { get; set; }
        public int? parentCategory { get; set; }
    }
}
