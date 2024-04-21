using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Best.Models
{

    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public required string Name { get; set; }

        [Required]
        [DisplayName("Display Order")]
        [Range(1,100)]

        public int DisplayOrder { get; set; }


    }
}
