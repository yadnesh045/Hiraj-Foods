

using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
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

        private readonly IUnitOfWorks unitOfWorks;

        public AdminController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginData Vm)
        {

            if (Vm != null)
            {
                string enteredEmail = Vm.EnteredEmail;
                string enteredPassword = Vm.EnteredPassword;

                var Admin = unitOfWorks.Admin.GetByEmail(enteredEmail);

                if (Admin != null && Admin.Password == enteredPassword)
                {
                    return RedirectToAction("dashboard", "Admin");
                }

                else
                {
                    return View();
                }

            }
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
