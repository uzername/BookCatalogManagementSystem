using System.ComponentModel;

namespace BCMS_Backend.Entities
{
    public class Category:BaseEntity
    {
        public String CategoryName { get; set; }
        public int? ParentCategory { get; set; }
        [Description ("ignore")]
        public String? ParentCategoryName { get; set; }
    }
}
