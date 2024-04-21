using Best.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Best.Models.ViewModels
{
    public class OrderVm
    {
    

        public OrderHeader OrderHeader { get; set; }

        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}