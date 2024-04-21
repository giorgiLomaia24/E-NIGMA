using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Best.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Book Title")]
        [Required(ErrorMessage = "Title is required")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "ISBN is required")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string? Author { get; set; }

        [DisplayName("List Price")]
        [Range(1, 1000, ErrorMessage = "List Price must be between 1 and 1000")]
        public double? ListPrice { get; set; }

        [DisplayName("Price for 1-50")]
        [Range(1, 1000, ErrorMessage = "Price for 1-50 must be between 1 and 1000")]
        public double? Price { get; set; }

        [DisplayName("Price for 50+")]
        [Range(1, 1000, ErrorMessage = "Price for 50+ must be between 1 and 1000")]
        public double? Price50 { get; set; }

        [DisplayName("Price for 100+")]
        [Range(1, 1000, ErrorMessage = "Price for 100+ must be between 1 and 1000")]
        public double? Price100 { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? ImageUrl { get; set; }


        [ValidateNever]
        public string? Status { get; set; }
    }
}
