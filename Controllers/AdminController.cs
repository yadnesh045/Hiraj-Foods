

using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class AdminController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWorks unitOfWorks;

        public AdminController(IUnitOfWorks unitOfWorks, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitOfWorks = unitOfWorks;
            this._webHostEnvironment = _webHostEnvironment;
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




        [HttpGet]
        public IActionResult Banner()
        {
            var Banner = unitOfWorks.Banner.GetAll().ToList();
            return View(Banner);
        }

        [HttpGet]
        public IActionResult AddBanner()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddBanner(Banner banner, IFormFile File)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (File != null)
                {
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"BannerImages");
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        File.CopyTo(fileStream);
                    }

                    var Cl = @"\BannerImages\" + filename;

                    var banner1 = new Banner
                    {
                        Flavour_title = banner.Flavour_title,
                        Banner_Img = Cl,
                    };

                    unitOfWorks.Banner.Add(banner1);
                    unitOfWorks.Save();

                    TempData["Success"] = "Banner Added Succefully";
                    return View();
                }
            }
          
                TempData["Error"] = "Banner Not Added";
 
            return View();
        }

		[HttpGet]
		public IActionResult DeleteBanner()
		{
			return View();
		}

		[HttpPost]
		public IActionResult DeleteBanner(int id)
		{
            var banner = unitOfWorks.Banner.GetById(id);
            if (banner == null)
            {
                return NotFound();
            }

            string logoImagePath = banner.Banner_Img;


            if (!string.IsNullOrEmpty(logoImagePath))
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string logoFilePath = Path.Combine(wwwRootPath, logoImagePath.TrimStart('\\'));
                if (System.IO.File.Exists(logoFilePath))
                {
                    System.IO.File.Delete(logoFilePath);
                }
            }
            else
            {
                TempData["Error"] = "Banner Not Deleted";
                return RedirectToAction("Banner", "Admin");
            }
            unitOfWorks.Banner.Remove(banner);
            unitOfWorks.Save();

            TempData["Success"] = "Banner Deleted Succefully";
            return RedirectToAction("Banner","Admin");
		}

	}
}
