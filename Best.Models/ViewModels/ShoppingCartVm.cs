using Best.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Best.Models.ViewModels
{
    public class ShoppingCartVm
    {
    
        public IEnumerable<ShoppingCart>? ShoppingCartList { get; set; }

        public OrderHeader OrderHeader { get; set; }
        public double OrderTotal { get; set; }
    }
}