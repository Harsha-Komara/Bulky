using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Bulky.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Bulky.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController(IUnitOfWork db,IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = db;
        private readonly string wwwRootPath = webHostEnvironment.WebRootPath;

        public IActionResult Index()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return View(companies);
        }
        [ActionName("Create")]
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            if (id == null)
            {
                return View(company);
            }
            company = _unitOfWork.Company.Get(u => u.Id == id);
            return View(company);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult Upsert(Company company)
        {

            if (ModelState.IsValid) 
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    TempData["Success"] = "Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    TempData["Success"] = "Updated Successfully";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            
            return View(company);
        }
        
        #region API Calls
        [HttpGet]
        public IActionResult GetData()
        {
            List<Company> companies = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = companies });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Company company= _unitOfWork.Company.Get(u => u.Id == id);
            if(company == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            _unitOfWork.Company.Remove(company);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
