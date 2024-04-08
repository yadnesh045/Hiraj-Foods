

using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class AdminController : Controller
    {
       

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
            var products = unitOfWorks.Product.GetAll().ToList();

            var productPrice = products.Select(p => p.ProductPrice).ToList();
            ViewBag.ProductPrices = productPrice;


            var flavors = products.Select(p => p.ProductFlavour).ToList();
            ViewBag.Flavors = flavors;


         //   var FeedBacks = unitOfWorks.FeedBack.GetAll().ToList();
         //   ViewBag.FeedBacks = FeedBacks;



            return View(products);
        }


        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddProduct(AddProductVM productVM)
        {
          
            return View();
        }

        [HttpGet]
        public IActionResult Enquiry()
        {
            var Enquires = unitOfWorks.Enquiry.GetAll().ToList();
            return View(Enquires);
        }

        [HttpPost]
        public IActionResult Enquiry(Enquiry Enq)
        {

            if(ModelState.IsValid)
            {
                
                unitOfWorks.Enquiry.Add(Enq);
                unitOfWorks.Save();
            }
            else
            {
                Console.WriteLine("-----------------------------------------------");
                return RedirectToAction("Enquiry" , "Rahul");
            }

            TempData["Enquiry"] = "Enquiry Sent to Admin";
            return RedirectToAction("Enquiry", "Rahul");
        }


        [HttpGet]
        public IActionResult Feedback()
        {
            var Feedback = unitOfWorks.Feedback.GetAll().ToList();
            return View(Feedback);
        }


        [HttpPost]
        public IActionResult Feedback(FeedBack Enq)
        {

            if (ModelState.IsValid)
            {

                unitOfWorks.Feedback.Add(Enq);
                unitOfWorks.Save();
            }
            else
            {
                Console.WriteLine("-----------------------------------------------");
                return RedirectToAction("Feedback", "Rahul");
            }

            TempData["Feedback"] = "Feedback Sent to Admin";
            return RedirectToAction("Feedback", "Rahul");
        }



    }
}
