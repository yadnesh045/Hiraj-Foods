

using Azure.Core;
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



        //  add product method to save product data in database with url in db and actual file in wwwroot folder

        [HttpPost]
        public IActionResult AddProduct(AddProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductName = productVM.ProductName,
                    ProductFlavour = productVM.ProductFlavour,
                    ProductPrice = productVM.ProductPrice,
                    ProductNutrition = productVM.ProductNutrition,
                    ProductWeight = productVM.ProductWeight,
                    ProductIngredient = productVM.ProductIngredient,
                    ProductDescription = productVM.ProductDescription,
                };

                if (productVM.ProductFlavourImage != null && productVM.ProductFlavourImage.Length > 0)
                {
                    var file = productVM.ProductFlavourImage;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", "ProductFlavourImages", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ProductFlavourImageUrl = Path.Combine("/Db_Images", "ProductFlavourImages", fileName).Replace("\\", "/"); ;

                }


                if (productVM.ProductImage != null && productVM.ProductImage.Length > 0)
                {
                    var file = productVM.ProductImage;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", "ProductImages", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    product.ProductImageUrl = Path.Combine("/Db_Images", "ProductImages", fileName).Replace("\\", "/"); ;
                }



                unitOfWorks.Product.Add(product);
                unitOfWorks.Save();

            }

            TempData["Message"] = "Product added successfully!";
            return RedirectToAction("ViewProduct");
        }


        [HttpGet]
        public IActionResult ViewProduct()
        {
            var products = unitOfWorks.Product.GetAll().ToList();
            return View(products);
        }


        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = unitOfWorks.Product.GetById(id);
            return View(product);
        }


        // edit product method to update product data in database with url in db and delete the previous file and add new file in wwwroot folder
        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            if (ModelState.IsValid)
            {
                var productInDb = unitOfWorks.Product.GetById(product.Id);

                productInDb.ProductName = product.ProductName;
                productInDb.ProductFlavour = product.ProductFlavour;
                productInDb.ProductPrice = product.ProductPrice;
                productInDb.ProductNutrition = product.ProductNutrition;
                productInDb.ProductWeight = product.ProductWeight;
                productInDb.ProductIngredient = product.ProductIngredient;
                productInDb.ProductDescription = product.ProductDescription;

                if (product.ProductFlavourImage != null && product.ProductFlavourImage.Length > 0)
                {
                    var file = product.ProductFlavourImage;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", "ProductFlavourImages", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productInDb.ProductFlavourImageUrl = Path.Combine("Db_Images", "ProductFlavourImages", fileName).Replace("\\", "/"); ;

                }

                if(product.ProductImage != null && product.ProductImage.Length > 0)
                {
                    var file = product.ProductImage;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", "ProductImages", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productInDb.ProductImageUrl = Path.Combine("Db_Images", "ProductImages", fileName).Replace("\\", "/"); ;
                }

                unitOfWorks.Product.Update(productInDb);
                unitOfWorks.Save();

            }

            TempData["Message"] = "Product Updated successfully!";
            return RedirectToAction("ViewProduct");

        }





        // delete product method to delete product data in database with url in db and delete the file from wwwroot folder
        [HttpGet]
        public IActionResult DeleteProduct(int id)
        {
            var product = unitOfWorks.Product.GetById(id);

            if (product != null)
            {
                var productFlavourImage = product.ProductFlavourImageUrl;
                var productImage = product.ProductImageUrl;

                if (productFlavourImage != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productFlavourImage);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                if (productImage != null)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productImage);
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                unitOfWorks.Product.Remove(product);
                unitOfWorks.Save();
            }

            TempData["Message"] = "Product deleted successfully!";
            return RedirectToAction("ViewProduct");
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
