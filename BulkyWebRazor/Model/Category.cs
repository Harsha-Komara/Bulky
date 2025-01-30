using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace BulkyWebRazor.Model
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name is required")]
        [DisplayName("Category Name")]
        [MaxLength(30, ErrorMessage = "Category Name should be less than 30 characters")]
        public required string Name { get; set; }
        [Required(ErrorMessage = "Display Order is required")]
        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "The Display Order must be between 1 - 100.")]
        public int DisplayOrder { get; set; }
    }
}
