using System.ComponentModel;

namespace BCMS_FrontendBlazor.Models
{
    public class Category 
    {
        public int Id { get; set; }
        public String CategoryName { get; set; }
        public int? ParentCategory { get; set; }
        
        public String? ParentCategoryName { get; set; }
    }
}
