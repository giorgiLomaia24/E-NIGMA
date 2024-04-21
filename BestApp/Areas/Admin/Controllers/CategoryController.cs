using System.Data.Common;
using BestApp.DataAccess.Data;
using Best.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Best.DataAccess.IRepository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Best.Utility;

namespace BestApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]

public class CategoryController : Controller
{
    private readonly ICategoryRepository catRepo;
    public CategoryController(ICategoryRepository db)
    {
        catRepo = db;
    }
    public IActionResult Index()
    {
        List<Category> categoryList = catRepo.GetAll().ToList();
        return View(categoryList);
    }

    public IActionResult Create(){
        return View();
    }

    [HttpPost]
     public IActionResult Create(Category obj){
        if(catRepo.GetAll().Any(c => c.DisplayOrder == obj.DisplayOrder)){
            ModelState.AddModelError("DisplayOrder","Display Order of" + " " + obj.DisplayOrder + " " + "already exists in our records, please pick the new number or update the existing one" );
           
        }
          if (catRepo.GetAll().AsEnumerable().Any(c => string.Equals(c.Name, obj.Name, StringComparison.OrdinalIgnoreCase)))
    {
        ModelState.AddModelError("Name", $"Category named '{obj.Name}' already exists in our records. Please pick a new name or update the existing one.");
    }


      
        if(ModelState.IsValid){
         catRepo.Add(obj);
         catRepo.Save();
            TempData["success"] = $"{obj.Name} was added succesfully to the category records";
        return RedirectToAction("Index");

        }

        return View();
       
    }

    public IActionResult Edit(int? id){
        if( id == null || id == 0 ){
            return NotFound();
        }

        // Category? category = _db.Categories.Find(id);
        Category? category = catRepo.Get(c => c.Id == id);
        // Category? category = _db.Categories.Where(c => c.Id == id);

        if (category == null){
            return NotFound();
        }
        return View(category);
    }


   [HttpPost]
public IActionResult Edit(Category obj)
{
    Category? category = catRepo.Get(c => c.Id == obj.Id);

    // Other code...

    if (catRepo.GetAll().Any(c => c.DisplayOrder == obj.DisplayOrder))
    {
        if (obj.DisplayOrder != category?.DisplayOrder)
        {
            ModelState.AddModelError("DisplayOrder", "Display Order of " + obj.DisplayOrder + " already exists in our records, please pick the new number or update the existing one");
        }
    }

    if (ModelState.IsValid)
    {
        catRepo.Update(obj);
        catRepo.Save();
        return RedirectToAction("Index");
    }

    return View();
}


    //  public IActionResult Delete(int? id){
    //     if( id == null || id == 0 ){
    //         return NotFound();
    //     }

    //     Category? category = _db.Categories.Find(id);
    //     // Category? category = _db.Categories.FirstOrDefault(c => c.Id == id);
    //     // Category? category = _db.Categories.Where(c => c.Id == id);

    //     if (category == null){
    //         return NotFound();
    //     }
    //     return View(category);
    // }

      [HttpPost,ActionName("Delete")]
     public IActionResult DeletePOST(int? id){
            Category? category = catRepo.Get(c => c.Id == id);
            if(category == null){
            return NotFound();
            }
        
        
         catRepo.Remove(category);
        TempData["delSuc"] = $"{category.Name} was deleted from category records. Action can not be undone!";
        catRepo.Save();
        return RedirectToAction("Index");

       
       
    }




}
