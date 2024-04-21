using Best.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Best.Models.ViewModels
{
    public class ProductVM
    {
       
        public Product? Product { get; set; } // Add '?' to declare the property as nullable


        [ValidateNever]
        public IEnumerable<SelectListItem>? CategoryList { get; set; } // Add '?' to declare the property as nullable
    }
}

