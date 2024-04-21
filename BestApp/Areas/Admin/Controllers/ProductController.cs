
using Best.Models;
using Microsoft.AspNetCore.Mvc;
using Best.DataAccess.IRepository;
using Best.DataAccess.IRepository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Best.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Best.Utility;
namespace BestApp.Areas.Admin.Controllers;



[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class ProductController : Controller
{
    private readonly IProductRepository catRepo;
    private readonly ICategoryRepository categoryRepository;

    private readonly IWebHostEnvironment _iwebhost;
    public ProductController(IProductRepository db, ICategoryRepository categoryRepo, IWebHostEnvironment iwebhost)
    {
        catRepo = db;
        categoryRepository = categoryRepo;
        _iwebhost = iwebhost;
    }
    public IActionResult Index()
    {
        List<Product> productList = catRepo.GetAll(includeProperties:"Category").ToList();
        return View(productList);
    }

    public IActionResult Upsert(int? id)
    {



        ProductVM productVM = new()
        {
            CategoryList = categoryRepository.GetAll().ToList().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            }),
            Product = new Product()

        };

        if (id == null || id == 0)
        {
            return View(productVM);

        }
        else
        {
            productVM.Product = catRepo.Get(p => p.Id == id);

            return View(productVM);
        }
    }

    [HttpPost]
public IActionResult Upsert(ProductVM productVM, IFormFile? file)
{
    if (ModelState.IsValid)
    {
        string wwwRootPath = _iwebhost.WebRootPath;
        if (file != null)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string productDirectory = Path.Combine(wwwRootPath, "images", "product");
            string filePath = Path.Combine(productDirectory, fileName);

            if (!Directory.Exists(productDirectory))
            {
                Directory.CreateDirectory(productDirectory);
            }

            if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
            {
                // delete the old image
                var oldPath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('/')); // TrimStart('/') to remove the leading slash

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }
            }

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            productVM.Product.ImageUrl = "/images/product/" + fileName;
        }

        if (productVM.Product.Id == 0)
        {
            catRepo.Add(productVM.Product);
            TempData["success"] = $"{productVM.Product.Title} was added successfully to the book records";
        }
        else
        {
            catRepo.Update(productVM.Product);
            
            TempData["success"] = $"{productVM.Product.Title} was updated successfully inside the book records";
        }

        catRepo.Save();
        return RedirectToAction("Index");
    }

    productVM.CategoryList = categoryRepository.GetAll().Select(u => new SelectListItem
    {
        Text = u.Name,
        Value = u.Id.ToString()
    });

    return View(productVM);
}




// end of the post method


[HttpPost, ActionName("Delete")]
public IActionResult DeletePOST(int? id)
{
    Product? product = catRepo.Get(c => c.Id == id);
    if (product == null)
    {
        return NotFound();
    }


    catRepo.Remove(product);
    TempData["delSuc"] = $"{product.Title} was deleted from book records. Action can not be undone!";
    catRepo.Save();
    return RedirectToAction("Index");



}

#region  APICALLs


[HttpGet]
public IActionResult GetALL(){
        List<Product> productList = catRepo.GetAll(includeProperties:"Category").ToList();
        return Json(new { data = productList });


}

[HttpDelete]
public IActionResult Delete(int? id){

        var productTobedeleted = catRepo.Get(p => p.Id == id);

        if(productTobedeleted == null){
            return Json(new { success = false, message = "Error while deleting" });

        }

           var oldPath = Path.Combine(_iwebhost.WebRootPath, productTobedeleted.ImageUrl.TrimStart('/')); // TrimStart('/') to remove the leading slash

                if (System.IO.File.Exists(oldPath))
                {
                    System.IO.File.Delete(oldPath);
                }

        catRepo.Remove(productTobedeleted);
        catRepo.Save();

        return Json(new { success = true, message = "product was deleted" });


        


}

#endregion


}



