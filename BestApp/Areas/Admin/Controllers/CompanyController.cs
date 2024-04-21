
using Best.Models;
using Microsoft.AspNetCore.Mvc;

using Best.DataAccess.IRepository.IRepository;

using Microsoft.AspNetCore.Authorization;
using Best.Utility;
namespace BestApp.Areas.Admin.Controllers;



[Area("Admin")]
[Authorize(Roles = SD.Role_Admin)]
public class CompanyController : Controller
{
    private readonly ICompanyRepository catRepo;
  
    public CompanyController(ICompanyRepository db)
    {
        catRepo = db;
      
    }
    public IActionResult Index()
    {
        List<Company> companyList = catRepo.GetAll().ToList();
        return View(companyList);
    }

    public IActionResult Upsert(int? id)
    {


        if (id == null || id == 0)
        {
            return View(new Company());

        }
        else
        {
            Company company = catRepo.Get(c => c.Id == id);

            return View(company);
        }
    }

    [HttpPost]
public IActionResult Upsert(Company company)
{
    if (ModelState.IsValid)
    {
       
        if (company.Id == 0)
        {
            catRepo.Add(company);
            TempData["success"] = $"{company.Name} was added successfully to the book records";
        }
        else
        {
            catRepo.Update(company);
            
            TempData["success"] = $"{company.Name} was updated successfully inside the book records";
        }

        catRepo.Save();
        return RedirectToAction("Index");
    }


    return View(company);
}




// end of the post method


[HttpPost, ActionName("Delete")]
public IActionResult DeletePOST(int? id)
{
    Company? company = catRepo.Get(c => c.Id == id);
    if (company == null)
    {
        return NotFound();
    }


    catRepo.Remove(company);
    TempData["delSuc"] = $"{company.Name} was deleted from book records. Action can not be undone!";
    catRepo.Save();
    return RedirectToAction("Index");



}

#region  APICALLs


[HttpGet]
public IActionResult GetALL(){
        List<Company> companyList = catRepo.GetAll().ToList();
        return Json(new { data = companyList });


}

[HttpDelete]
public IActionResult Delete(int? id){

        var companyTobedeleted = catRepo.Get(c => c.Id == id);

        if(companyTobedeleted == null){
            return Json(new { success = false, message = "Error while deleting" });

        }

        

        catRepo.Remove(companyTobedeleted);
        catRepo.Save();

        return Json(new { success = true, message = "product was deleted" });


        


}

#endregion


}



