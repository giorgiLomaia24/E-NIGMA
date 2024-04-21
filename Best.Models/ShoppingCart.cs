using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Best.Models;

public class ShoppingCart
{
    public int Id { get; set; }
    
    public int? ProductId { get; set; }
    [ForeignKey("ProductId")]
    [ValidateNever]
    public Product Product { get; set; }
  
     [Range(1,100, ErrorMessage = "You are not allwed to choose any of the quantity below 1 and above 100")]
    public int Count { get; set; }   

    public string ApplicationUserId { get; set; }
    [ForeignKey("ApplicationUserId")]
    [ValidateNever]
    public ApplicationUser ApplicationUser { get; set; }

    [NotMapped]
    public double Price { get; set; }





}
