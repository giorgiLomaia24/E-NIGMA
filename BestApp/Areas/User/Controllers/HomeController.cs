using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Best.Models;
using Best.DataAccess.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Best.DataAccess.IRepository.IRepository;
using Best.Utility;
using Best.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace BestApp.Areas.User.Controllers;

[Area("User")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductRepository _iproductRepository;

    private readonly IShoppingCartRepository _ishoppingCartRepo;

    public HomeVM HomeVM { get; set; }

    public HomeController(ILogger<HomeController> logger,IProductRepository iproductRepository,IShoppingCartRepository ishoppingCartRepository)
    {
        _logger = logger;
        _iproductRepository = iproductRepository;
        _ishoppingCartRepo = ishoppingCartRepository;
    }

    public IActionResult Index()
    {
          var claimsIdentity = (ClaimsIdentity)User.Identity;
           var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

               if (claim != null && claim.Value != null) {
                 HttpContext.Session.SetInt32(SD.SessionCart, _ishoppingCartRepo.GetAll(s => s.ApplicationUserId == claim.Value).Sum(s => s.Count));

           }

    
 HomeVM = new()
        {
            Featured = _iproductRepository.GetAll(includeProperties:"Category").Where(b => b.Status == "Featured"),
            New =  _iproductRepository.GetAll(includeProperties:"Category").Where(b => b.Status == "New"),
            New2 =  _iproductRepository.GetAll(includeProperties:"Category").Where(b => b.Status == "New2"),
            Off50 =  _iproductRepository.GetAll(includeProperties:"Category").Where(b => b.Status == "50off"),

           Products =  _iproductRepository.GetAll(includeProperties:"Category"),
           };

        return View(HomeVM);
    }
      public IActionResult Details(int productId)
    {
        ShoppingCart cart = new()
        {
            Product = _iproductRepository.Get(p => p.Id == productId, includeProperties: "Category"),
            Count = 1,
            ProductId = productId

        };

        return View(cart);
    }

      [HttpPost]
      [Authorize]
       public IActionResult AddToCart(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

        shoppingCart.ApplicationUserId = userId;

        ShoppingCart shoppingCartFromDb = _ishoppingCartRepo.Get(s => s.ProductId == shoppingCart.ProductId && s.ApplicationUserId == userId);
        if(shoppingCartFromDb != null){
            shoppingCartFromDb.Count += shoppingCart.Count;
            _ishoppingCartRepo.Update(shoppingCartFromDb);
            _ishoppingCartRepo.Save();
            return RedirectToAction("Index");

        }else{
       _ishoppingCartRepo.Add(shoppingCart);
               _ishoppingCartRepo.Save();
    HttpContext.Session.SetInt32(SD.SessionCart, _ishoppingCartRepo.GetAll(s => s.ApplicationUserId == userId).Sum(s => s.Count));

        return RedirectToAction("Index");
        }

       
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }


    #region API CALLS


[HttpPost("AddToCart")]
[Authorize]
public IActionResult AddToCartHT([FromBody] ShoppingCartData shoppingCartData)
{
    if (shoppingCartData == null)
    {
        return BadRequest("Invalid data");
    }

    var claimsIdentity = (ClaimsIdentity)User.Identity;
    var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

    ShoppingCart shoppingCartFromDb = _ishoppingCartRepo.Get(s => s.ProductId == shoppingCartData.ProductId && s.ApplicationUserId == userId);

    if (shoppingCartFromDb != null)
    {
        shoppingCartFromDb.Count += shoppingCartData.Count;
        _ishoppingCartRepo.Update(shoppingCartFromDb);
    }
    else
    {
        ShoppingCart newShoppingCart = new ShoppingCart
        {
            ProductId = shoppingCartData.ProductId,
            Count = shoppingCartData.Count,
            ApplicationUserId = userId
        };
        _ishoppingCartRepo.Add(newShoppingCart);
    }

    _ishoppingCartRepo.Save();
var cartCount = _ishoppingCartRepo.GetAll(s => s.ApplicationUserId == userId).Sum(s => s.Count);
HttpContext.Session.SetInt32(SD.SessionCart, cartCount);


    
    return Ok(new { cartCount });
}

[HttpGet]

public IActionResult GetBooksBySearch(string query)
{
    // Retrieve books that start with the specified query
    var books = _iproductRepository.GetAll(b => b.Title.StartsWith(query));

    // Return the filtered books as JSON
    return Ok(books);
}





public class ShoppingCartData
{
    public int ProductId { get; set; }
    public int Count { get; set; }
}


    #endregion



}
