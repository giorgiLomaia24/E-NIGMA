using Best.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Best.Models.ViewModels
{
    public class HomeVM
    {       
        public IEnumerable<Product>? Products { get; set; } 

        public IEnumerable<Product>? Featured { get; set; } 

        public IEnumerable<Product>? New { get; set; } 
        public IEnumerable<Product>? New2 { get; set; } 

        public IEnumerable<Product>? Off50 { get; set; } 
    }
}
