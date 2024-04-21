using Best.DataAccess.IRepository.IRepository;
using Best.Models;
using Best.Models.ViewModels;
using Best.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BestApp.Areas.Admin.Controllers;

[Area("Admin")]
public class OrderController : Controller {

     private readonly IShoppingCartRepository _ishopRepo;



        private readonly IOrderHeaderRepository _iorderHeader;
        private readonly IOrderDetailRepository _iorderDetail;

        private readonly IApplicationUserRepository _iAppUserRepo;
        [BindProperty]
        public ShoppingCartVm ShoppingCartVm { get; set; }

        [BindProperty]
        public OrderVm OrderVm { get; set; }

        public OrderController(IShoppingCartRepository ishopRepo, IApplicationUserRepository iappUser, IOrderHeaderRepository iorderheader, IOrderDetailRepository iorderDetail)
        {
            _ishopRepo = ishopRepo;
            _iAppUserRepo = iappUser;
            _iorderHeader = iorderheader;
            _iorderDetail = iorderDetail;
        }


        public IActionResult Index(){

        return View();

        }

        public IActionResult Details(int orderId)
{
      OrderVm = new()
    {
        OrderHeader = _iorderHeader.Get(u => u.Id == orderId, includeProperties: "ApplicationUser"),
        OrderDetails = _iorderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties: "Product")
    };
    return View(OrderVm);
}

    [HttpPost]
    [Authorize(Roles = SD.Role_Employy + "," + SD.Role_Admin)]

        public IActionResult UpdateOrderDetails()
{
        var orderHeaderFromDb = _iorderHeader.Get(u => u.Id == OrderVm.OrderHeader.Id);
        orderHeaderFromDb.Name = OrderVm.OrderHeader.Name;
        orderHeaderFromDb.PhoneNumber = OrderVm.OrderHeader.PhoneNumber;
        orderHeaderFromDb.StreetAddress = OrderVm.OrderHeader.StreetAddress;
        orderHeaderFromDb.City = OrderVm.OrderHeader.City;
        orderHeaderFromDb.State = OrderVm.OrderHeader.State;
        orderHeaderFromDb.PostalCode = OrderVm.OrderHeader.PostalCode;
        if(!string.IsNullOrEmpty(orderHeaderFromDb.TrackingNumber)){
        orderHeaderFromDb.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;

        }
        if(!string.IsNullOrEmpty(orderHeaderFromDb.Carrier)){
        orderHeaderFromDb.Carrier = OrderVm.OrderHeader.Carrier;

        }

        _iorderHeader.Update(orderHeaderFromDb);
        _iorderHeader.Save();

    return RedirectToAction(nameof(Details), new {orderId = orderHeaderFromDb.Id});
}



[HttpPost]
[Authorize(Roles = SD.Role_Employy + "," + SD.Role_Admin)]
public IActionResult StartProcessing(){
        _iorderHeader.UpdateStatus(OrderVm.OrderHeader.Id, SD.StatusInProcess);
        _iorderHeader.Save();
        TempData["statussuc"] = $"Order is now in process";
        return RedirectToAction(nameof(Details),new{orderId = OrderVm.OrderHeader.Id});
}


[HttpPost]
[Authorize(Roles = SD.Role_Employy + "," + SD.Role_Admin)]
public IActionResult StartShipping(){
    var orderHeader = _iorderHeader.Get(u => u.Id == OrderVm.OrderHeader.Id);
    
    // Check if orderHeader is null
    if (orderHeader == null)
    {
        // Handle the case where orderHeader is not found
        return NotFound();
    }

    orderHeader.TrackingNumber = OrderVm.OrderHeader.TrackingNumber;
    orderHeader.Carrier = OrderVm.OrderHeader.Carrier;
    orderHeader.OrderStatus = SD.StatusShipped;
    orderHeader.ShippingDate = DateTime.Now;

    if(orderHeader.PaymentStatus == SD.PaymentStatusDelayedPayment){
        orderHeader.PaymentDate = DateTime.Now.AddDays(30).Date;
    }

    try
    {
        _iorderHeader.Update(orderHeader);
        _iorderHeader.Save();
        TempData["statusshipped"] = $"Order shipped successfully";
    }
    catch (Exception ex)
    {
        // Log the exception or handle it appropriately
        TempData["statusshipped"] = $"Error: {ex.Message}";
    }

    // Ensure that 'Details' action is correctly implemented and that 'orderId' is a valid parameter
    return RedirectToAction(nameof(Details), new { orderId = OrderVm.OrderHeader.Id });
}


public IActionResult CancelOrder(){
        return View();
}




        #region API CALLS

       [HttpGet]
     
public IActionResult GetAll(string status)
{
    IEnumerable<OrderHeader> orderHeaders = _iorderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
    switch (status)
    {
        case "pending":
            orderHeaders = orderHeaders.Where(o => o.PaymentStatus == SD.PaymentStatusDelayedPayment);
            break;
        case "inprocess":
            orderHeaders = orderHeaders.Where(o => o.PaymentStatus == SD.StatusInProcess);
            break;
        case "completed":
            orderHeaders = orderHeaders.Where(o => o.PaymentStatus == SD.StatusShipped);
            break;
        case "approved":
            orderHeaders = orderHeaders.Where(o => o.PaymentStatus == SD.StatusApproved);
            break;
        default:
            break;
    }
    
    // Return the filtered orderHeaders collection
    return Json(new { data = orderHeaders });
}





 #endregion

}
