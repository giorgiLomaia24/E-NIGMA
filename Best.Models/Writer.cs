using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Best.Models
{
    public class Writer
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("Book Title")]
        [Required(ErrorMessage = "Title is required")]
        public String Name { get; set; }
        

        public int? ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string? ImageUrl { get; set; }


        [ValidateNever]
        public string? Description { get; set; }
    }
}
