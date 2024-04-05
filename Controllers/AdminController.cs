using Hiraj_Foods.Anurag_Data;
using Hiraj_Foods.Models.View_Model;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminController(ApplicationDbContext db)
        {

            _db = db;

        }
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginData Vm)
        {
            return View();
        }

        public IActionResult dashboard()
        {
            var products = _db.Products.ToList();

            var productPrice = products.Select(p => p.ProductPrice).ToList();
            ViewBag.ProductPrices = productPrice;


            var flavors = products.Select(p => p.ProductFlavour).ToList();
            ViewBag.Flavors = flavors;



            return View(products);
        }



    }
}
