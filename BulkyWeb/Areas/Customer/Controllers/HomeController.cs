using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Bulky.Controllers
{
    [Area("Customer")]
    public class HomeController(IUnitOfWork db, ILogger<HomeController> logger) : Controller
    {
        private readonly ILogger<HomeController> _logger = logger;
        private readonly IUnitOfWork _unitOfWork = db;


        public IActionResult Index()
        {
            List<Product> ProductList = _unitOfWork.Products.GetAll(includeProperties: "Category").ToList();
            return View(ProductList);
        }

        public IActionResult Details(int? productId)
        {
            Product Product = _unitOfWork.Products.Get(u=>u.Id== productId, includeProperties: "Category");
            return View(Product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Harsha()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
