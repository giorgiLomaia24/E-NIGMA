using System.Security.Claims;
using Best.DataAccess.IRepository.IRepository;
using Best.Models;
using Best.Models.ViewModels;
using Best.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Stripe.Checkout;

namespace BestApp.Areas.User
{
    [Area("User")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IShoppingCartRepository _ishopRepo;



        private readonly IOrderHeaderRepository _iorderHeader;
        private readonly IOrderDetailRepository _iorderDetail;

        private readonly IApplicationUserRepository _iAppUserRepo;
        [BindProperty]
        public ShoppingCartVm ShoppingCartVm { get; set; }

        public CartController(IShoppingCartRepository ishopRepo, IApplicationUserRepository iappUser, IOrderHeaderRepository iorderheader, IOrderDetailRepository iorderDetail)
        {
            _ishopRepo = ishopRepo;
            _iAppUserRepo = iappUser;
            _iorderHeader = iorderheader;
            _iorderDetail = iorderDetail;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            ShoppingCartVm = new()
            {
                ShoppingCartList = _ishopRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            double orderTotalVar = 0.0;



            foreach (var cart in ShoppingCartVm.ShoppingCartList)
            {
                cart.Price = (double)getProductPriceByQuantity(cart);
                orderTotalVar += cart.Price * cart.Count;
            }

            ShoppingCartVm.OrderHeader.OrderTotal = orderTotalVar;

            return View(ShoppingCartVm);

        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            ShoppingCartVm = new()
            {
                ShoppingCartList = _ishopRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product"),
                OrderHeader = new()
            };

            double orderTotalVar = 0.0;



            ShoppingCartVm.OrderHeader.ApplicationUser = _iAppUserRepo.Get(u => u.Id == userId);

            ShoppingCartVm.OrderHeader.State = ShoppingCartVm.OrderHeader.ApplicationUser.State;
            ShoppingCartVm.OrderHeader.City = ShoppingCartVm.OrderHeader.ApplicationUser.City;
            ShoppingCartVm.OrderHeader.StreetAddress = ShoppingCartVm.OrderHeader.ApplicationUser.StreetAdress;
            ShoppingCartVm.OrderHeader.PostalCode = ShoppingCartVm.OrderHeader.ApplicationUser.PostalCode;
            ShoppingCartVm.OrderHeader.Name = ShoppingCartVm.OrderHeader.ApplicationUser.Name;
            ShoppingCartVm.OrderHeader.PhoneNumber = ShoppingCartVm.OrderHeader.ApplicationUser.PhoneNumber;

            foreach (var cart in ShoppingCartVm.ShoppingCartList)
            {
                cart.Price = (double)getProductPriceByQuantity(cart);
                orderTotalVar += cart.Price * cart.Count;
            }

            ShoppingCartVm.OrderHeader.OrderTotal = orderTotalVar;


            return View(ShoppingCartVm);

        }


        [HttpPost]
        [ActionName("Summary")]
      
public IActionResult SummaryPost()
{
    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

    ShoppingCartVm.ShoppingCartList = _ishopRepo.GetAll(u => u.ApplicationUserId == userId, includeProperties: "Product");
    ShoppingCartVm.OrderHeader.ApplicationUserId = userId;
    ShoppingCartVm.OrderHeader.OrderDate = System.DateTime.Now;
    ApplicationUser applicationUser = _iAppUserRepo.Get(u => u.Id == userId);

    double orderTotalVar = 0.0;

    foreach (var cart in ShoppingCartVm.ShoppingCartList)
    {
        cart.Price = (double)getProductPriceByQuantity(cart);
        orderTotalVar += cart.Price * cart.Count;
    }

    ShoppingCartVm.OrderHeader.OrderTotal = orderTotalVar;

    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
    {
        // it is a regular user
        ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusPending;
        ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
    }
    else
    {
        // it user related to company
        ShoppingCartVm.OrderHeader.OrderStatus = SD.StatusApproved;
        ShoppingCartVm.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
    }

    // Add order header to the database
    _iorderHeader.Add(ShoppingCartVm.OrderHeader);
    _iorderHeader.Save();

    // Add order details to the database
    foreach (var cart in ShoppingCartVm.ShoppingCartList)
    {
        OrderDetail orderDetail = new()
        {
            ProductId = (int)cart.ProductId,
            OrderHeaderId = ShoppingCartVm.OrderHeader.Id,
            Price = cart.Price,
            Count = cart.Count
        };

        _iorderDetail.Add(orderDetail);
    }
    _iorderDetail.Save();

    if (applicationUser.CompanyId.GetValueOrDefault() == 0)
    {
        // it is a regular user so we will start stripe logic right away
        var options = new Stripe.Checkout.SessionCreateOptions
        {
            SuccessUrl = $"http://localhost:5037/User/Cart/OrderConfirmation?id={ShoppingCartVm.OrderHeader.Id}",
            CancelUrl =  $"http://localhost:5037/User/Cart/Index",
            LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
            Mode = "payment",
        };

        foreach (var item in ShoppingCartVm.ShoppingCartList)
        {
            var sessionLineItemOption = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(item.Price * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Product.Title,

                    },
                },
                Quantity = item.Count
            };

            options.LineItems.Add(sessionLineItemOption);
        }
        var service = new SessionService();
        Session session = service.Create(options);

        _iorderHeader.UpdateStripePaymentId(ShoppingCartVm.OrderHeader.Id, session.Id, session.PaymentIntentId);
        _iorderHeader.Save();
        Response.Headers.Add("Location", session.Url);
        return new StatusCodeResult(303);
    }

    return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVm.OrderHeader.Id });
}


        public IActionResult OrderConfirmation(int id)
        {

            OrderHeader orderHeader = _iorderHeader.Get(o => o.Id == id, includeProperties: "ApplicationUser");
            if(orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment){
                // regular user
                var service = new SessionService();
                Session session = service.Get(orderHeader.SessionId);

                if(session.PaymentStatus.ToLower() == "paid"){
                    _iorderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
                    _iorderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
                    _iorderHeader.Save();

                }

                List<ShoppingCart> shoppingCarts = _ishopRepo.GetAll(o => o.ApplicationUserId == orderHeader.ApplicationUserId).ToList();
                _ishopRepo.RemoveRange(shoppingCarts);
                _ishopRepo.Save();

            }

            return View(id);
        }

  public IActionResult RemoveFromCart(int cartId)
        {
            var cartFromDb = _ishopRepo.Get(c => c.Id == cartId);
            HttpContext.Session.SetInt32(SD.SessionCart, _ishopRepo.GetAll(s => s.ApplicationUserId == cartFromDb.ApplicationUserId).Sum(s => s.Count) - cartFromDb.Count);

            _ishopRepo.Remove(cartFromDb);

            _ishopRepo.Save();


            return RedirectToAction("Index");


        }       
public IActionResult Plus(int cartId)
{
    var cartFromDb = _ishopRepo.Get(c => c.Id == cartId);
    if (cartFromDb == null)
    {
        // Handle case where shopping cart item does not exist
        // Redirect to a proper error page or return an error message
        return NotFound();
    }

    HttpContext.Session.SetInt32(SD.SessionCart, _ishopRepo.GetAll(s => s.ApplicationUserId == cartFromDb.ApplicationUserId).Sum(s => s.Count) + 1);

    // Increment the count
    cartFromDb.Count += 1;

    // Update the cart in the repository
    _ishopRepo.Update(cartFromDb);
    _ishopRepo.Save();

    // Return the view with the updated data
    return RedirectToAction("Index");
}

public IActionResult Minus(int cartId)
{
    var cartFromDb = _ishopRepo.Get(c => c.Id == cartId);
    if (cartFromDb == null)
    {
        // Handle case where shopping cart item does not exist
        // Redirect to a proper error page or return an error message
        return NotFound();
    }

    if (cartFromDb.Count <= 1)
    {
        HttpContext.Session.SetInt32(SD.SessionCart, _ishopRepo.GetAll(s => s.ApplicationUserId == cartFromDb.ApplicationUserId).Sum(s => s.Count) - cartFromDb.Count);
        _ishopRepo.Remove(cartFromDb);
    }
    else
    {
        cartFromDb.Count -= 1;
        HttpContext.Session.SetInt32(SD.SessionCart, _ishopRepo.GetAll(s => s.ApplicationUserId == cartFromDb.ApplicationUserId).Sum(s => s.Count) - 1);
        _ishopRepo.Update(cartFromDb);
    }
    _ishopRepo.Save();

    // Return the view with the updated data
    return RedirectToAction("Index");
}



        
       

        private double? getProductPriceByQuantity(ShoppingCart shoppingCart)
        {
            // Check if the ShoppingCart object or its Product property is null
    if (shoppingCart == null || shoppingCart.Product == null)
    {
        // Handle null case gracefully
        return null;
    }
            if (shoppingCart.Count <= 50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= 100)
            {
                return shoppingCart.Product.Price50;
            }
            else
            {
                return shoppingCart.Product.Price100;
            }
        }
        




    }

}

