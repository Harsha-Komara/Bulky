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
    public class ProductController(IUnitOfWork db,IWebHostEnvironment webHostEnvironment) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = db;
        private readonly string wwwRootPath = webHostEnvironment.WebRootPath;

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperties : "Category").ToList();
            return View(products);
        }
        [ActionName("Create")]
        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            //ViewBag.CategoryList = categoryList;
            //ViewData["CategoryList"] = categoryList;

            ProductVM productsVM = new()
            {
                Product=new Product(),
                CategoryList = categoryList
            };
            if  (id != null & id != 0){
                productsVM.Product = _unitOfWork.Products.Get(u => u.Id == id);
            }
            return View(productsVM);
        }
        [HttpPost, ActionName("Create")]
        public IActionResult Upsert(ProductVM productsVM, IFormFile? file)
        {

            if (file != null)
            {
                string filePath = Path.Combine(@"\Images\Product", Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                FileStream fileStream = new(wwwRootPath + filePath, FileMode.Create);
                file.CopyTo(fileStream);
                if(productsVM.Product.ImageUrl != null)
                {
                    FileInfo fi = new(wwwRootPath + productsVM.Product.ImageUrl);
                    if (fi.Exists)
                        fi.Delete();
                }
                productsVM.Product.ImageUrl = filePath;
            }

            if (ModelState.IsValid) 
            {
                if (productsVM.Product.Id == 0)
                {
                    _unitOfWork.Products.Add(productsVM.Product);
                    TempData["Success"] = "Created Successfully";
                }
                else
                {
                    _unitOfWork.Products.update(productsVM.Product);
                    TempData["Success"] = "Updated Successfully";
                }
                
                _unitOfWork.Save();
                
                return RedirectToAction("Index");
            }
            IEnumerable<SelectListItem> categoryList = _unitOfWork.Category.
                GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
            productsVM.CategoryList=categoryList;
            return View(productsVM);
        }
        
        #region API Calls
        [HttpGet]
        public IActionResult GetData()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperties : "Category").ToList();
            return Json(new { data = products });
        }
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product product = _unitOfWork.Products.Get(u => u.Id == id);
            if(product == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }
            if (product.ImageUrl != null)
            {
                FileInfo fi = new(wwwRootPath + product.ImageUrl);
                if (fi.Exists)
                    fi.Delete();
            }
            _unitOfWork.Products.Remove(product);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Deleted Successfully" });
        }
        #endregion
    }
}
