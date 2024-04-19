using Azure.Core;
using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Hiraj_Foods.Controllers
{

    [Authorize]
    public class AdminController : Controller
    {


        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWorks unitOfWorks;

        public AdminController(IUnitOfWorks unitOfWorks, IWebHostEnvironment _webHostEnvironment)
        {
            this.unitOfWorks = unitOfWorks;
            this._webHostEnvironment = _webHostEnvironment;
        }


        public void SetAdminData()
        {
            var AdminEmail = HttpContext.Session.GetString("AdminEmail");
            var AdminId = HttpContext.Session.GetInt32("AdminId");
            var Admin = unitOfWorks.Admin.GetByEmail(AdminEmail);
            ViewData["Admin"] = Admin;
        }





        public IActionResult Login()
        {
            return View();
        }





        public IActionResult dashboard()
        {
            SetAdminData();


            var products = unitOfWorks.Product.GetAll().ToList();
            var productPrice = products.Select(p => p.ProductPrice).ToList();
            ViewBag.ProductPrices = productPrice;

            var productNames = products.Select(p => p.ProductName).ToList();
            ViewBag.ProductNames = productNames;

            var flavors = products.Select(p => p.ProductFlavour).ToList();
            ViewBag.Flavors = flavors;

            // Extract Energy Values
            var energyValues = products.Select(p =>
            {
                // Assuming the format "Energy - value, Proteins - value, ..."
                var energyPart = p.ProductNutrition.Split(',').FirstOrDefault(s => s.Trim().StartsWith("Energy"));
                return energyPart?.Split('-').ElementAtOrDefault(1)?.Trim();
            }).ToList();

            ViewBag.EnergyValues = energyValues;



            var feedback = unitOfWorks.Feedback.GetAll().ToList();


            // Classify feedback messages
            int positiveCount = 0, negativeCount = 0, neutralCount = 0;
            foreach (var feedbackItem in feedback)
            {
                var sentiment = ClassifySentiment(feedbackItem.Message);
                switch (sentiment)
                {
                    case "Positive":
                        positiveCount++;
                        break;
                    case "Negative":
                        negativeCount++;
                        break;
                    default:
                        neutralCount++;
                        break;
                }
            }


            ViewBag.PositiveFeedbackCount = positiveCount;
            ViewBag.NegativeFeedbackCount = negativeCount;
            ViewBag.NeutralFeedbackCount = neutralCount;


            var enquiry = unitOfWorks.Enquiry.GetAll().ToList();

            var model = new Tuple<List<Product>, List<FeedBack>, List<Enquiry>>(products, feedback, enquiry);

            return View(model);
        }



        [HttpGet]
        public IActionResult AddProduct()
        {
            SetAdminData();
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
            var products = unitOfWorks.Product.GetAll().OrderByDescending(c => c.Id).ToList();

            SetAdminData();


            var model = new Tuple<List<Product>>(products);

            return View(model);

        }


        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = unitOfWorks.Product.GetById(id);

            SetAdminData();

            var model = new Tuple<Product>(product);

            return View(model);
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
                    productInDb.ProductFlavourImageUrl = Path.Combine("/Db_Images", "ProductFlavourImages", fileName).Replace("\\", "/"); ;

                }
                else
                {
                    productInDb.ProductFlavourImageUrl = productInDb.ProductFlavourImageUrl;
                }

                if (product.ProductImage != null && product.ProductImage.Length > 0)
                {
                    var file = product.ProductImage;
                    var fileName = Guid.NewGuid().ToString() + file.FileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Db_Images", "ProductImages", fileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productInDb.ProductImageUrl = Path.Combine("/Db_Images", "ProductImages", fileName).Replace("\\", "/"); ;
                }
                else
                {
                    productInDb.ProductImageUrl = productInDb.ProductImageUrl;
                }

                unitOfWorks.Product.Update(productInDb);
                unitOfWorks.Save();
                TempData["Message"] = "Product Updated successfully!";
                return RedirectToAction("ViewProduct");
            }
            else
            {

                TempData["Error"] = "Product Not Updated !!!";
                return RedirectToAction("ViewProduct");
            }


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
            var Enquires = unitOfWorks.Enquiry.GetAll().OrderByDescending(c => c.Id).ToList();

            SetAdminData();


            var model = new Tuple<List<Enquiry>>(Enquires);

            return View(model);
        }



        [HttpGet]
        public IActionResult Feedback()
        {
            var Feedback = unitOfWorks.Feedback.GetAll().OrderByDescending(c => c.Id).ToList();


            SetAdminData();

            var model = new Tuple<List<FeedBack>>(Feedback);

            return View(model);
        }

        [HttpGet]
        public IActionResult Banner()
        {
            var Banner = unitOfWorks.Banner.GetAll().ToList();


            SetAdminData();

            var model = new Tuple<List<Banner>>(Banner);
            return View(model);
        }

        [HttpGet]
        public IActionResult AddBanner()
        {
            SetAdminData();

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
                    SetAdminData();
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
        public IActionResult ViewContact()
        {
            SetAdminData();
            var contacts = unitOfWorks.Contact.GetAll().OrderByDescending(c => c.Id).ToList();




            var model = new Tuple<List<Contact>>(contacts);
            return View(model);
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
            return RedirectToAction("Banner", "Admin");
        }


        [HttpGet]
        public IActionResult ChangePassword()
        {

            SetAdminData();

            return View();
        }





        [HttpPost]
        public IActionResult ChangePassword(Admin admin)
        {

            var AdminEmailInDb = HttpContext.Session.GetString("AdminEmail");
            if (AdminEmailInDb == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            // get admin by email from db and check if entered password old password is same as in db then update the password
            var AdminInDb = unitOfWorks.Admin.GetByEmail(AdminEmailInDb);

            if (AdminInDb.Password == admin.Password)
            {
                AdminInDb.Password = admin.NewPassword;
                unitOfWorks.Admin.Update(AdminInDb);
                unitOfWorks.Save();
                TempData["Message"] = "Password Changed Successfully!";
                return RedirectToAction("dashboard", "Admin");
            }
            else
            {
                return RedirectToAction("dashboard", "Admin");
            }
        }


        [HttpGet]
        public IActionResult AccountSetting()
        {
            var AdminEmail = HttpContext.Session.GetString("AdminEmail");
            var Admin = unitOfWorks.Admin.GetByEmail(AdminEmail);

            SetAdminData();

            return View(Admin);
        }

        [HttpPost]
        public IActionResult AccountSetting(UpdateAdmin updateadmin)
        {
            if (ModelState.IsValid)
            {
                var AdminEmailInDb = HttpContext.Session.GetString("AdminEmail");
                if (AdminEmailInDb == null)
                {
                    return RedirectToAction("Login", "Admin");
                }

                var AdminInDb = unitOfWorks.Admin.GetByEmail(AdminEmailInDb);

                AdminInDb.FirstName = updateadmin.FirstName;
                AdminInDb.LastName = updateadmin.LastName;
                AdminInDb.Address = updateadmin.Address;
                AdminInDb.Mobile = updateadmin.Mobile;
                AdminInDb.State = updateadmin.State;
                AdminInDb.ZipCode = updateadmin.ZipCode;

                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (updateadmin.ProfilePicture != null)
                {
                    if (AdminInDb.ImageUrl != null)
                    {
                        //remove old image from wwwroot folder and db if new image is uploaded
                        string imagePath = AdminInDb.ImageUrl;
                        string imageFullPath = Path.Combine(wwwRootPath, imagePath.TrimStart('\\'));
                        if (System.IO.File.Exists(imageFullPath))
                        {
                            System.IO.File.Delete(imageFullPath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(updateadmin.ProfilePicture.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"Db_Images\ProfileImages");
                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        updateadmin.ProfilePicture.CopyTo(fileStream);
                    }

                    AdminInDb.ImageUrl = Path.Combine("/Db_Images", "ProfileImages", fileName).Replace("\\", "/");
                }

                unitOfWorks.Admin.Update(AdminInDb);
                unitOfWorks.Save();
                TempData["Success"] = "Account Updated Successfully!";
                return RedirectToAction("AccountSetting", "Admin");
            }
            else
            {
                TempData["Error"] = "Account Data Not Updated !";
                return RedirectToAction("AccountSetting", "Admin");
            }
        }


        [HttpPost]
        public IActionResult DeactivateAdmin()
        {
            var AdminEmail = HttpContext.Session.GetString("AdminEmail");
            if (AdminEmail == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                var Admin = unitOfWorks.Admin.GetByEmail(AdminEmail);

                Admin.IsActive = false;

                unitOfWorks.Admin.Update(Admin);
                unitOfWorks.Save();
                return RedirectToAction("Login", "Login");
            }
        }

        [HttpGet]
        public IActionResult ViewUser()
        {
            var user = unitOfWorks.Users.GetAll().ToList();

            SetAdminData();


            var model = new Tuple<List<User>>(user);

            return View(model);
        }

        [HttpGet]
        public IActionResult ViewCheckouts()
        {
            var user = unitOfWorks.Users.GetAll().ToList();

            // Order checkouts by CreatedDate in descending order
            var Checkouts = unitOfWorks.Checkout.GetAll().OrderByDescending(c => c.Date).ToList();



            SetAdminData();


            var model = new Tuple<List<User>, List<Checkout>>(user, Checkouts);
            return View(model);
        }


        //------------------------------------------------ User OPertaion By Admin -----------------------------------------------------

        //----------------------------------------------Edit User --------------------------------------------------------------------

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = unitOfWorks.Users.GetById(id);


            SetAdminData();

            var model = new Tuple<User>(user);

            return View(model);

        }

        [HttpPost]
        public IActionResult EditUser(User usr)
        {
            if (ModelState.IsValid)
            {
                var userInDb = unitOfWorks.Users.GetById(usr.Id);


                userInDb.FirstName = usr.FirstName;
                userInDb.LastName = usr.LastName;
                userInDb.Email = usr.Email;
                userInDb.Password = usr.Password;
                userInDb.Phone = usr.Phone;

                unitOfWorks.Users.Update(userInDb);
                unitOfWorks.Save();

            }

            TempData["Message"] = "User Updated successfully!";
            return RedirectToAction("ViewUser");

        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = unitOfWorks.Users.GetById(id);


            unitOfWorks.Users.Remove(user);
            unitOfWorks.Save();


            TempData["Message"] = "User deleted successfully!";
            return RedirectToAction("ViewUser");
        }



        private string ClassifySentiment(string message)
        {
            // Define positive and negative keywords
            var positiveKeywords = new List<string> { "good", "great", "excellent", "awesome", "delicious", "fresh", "fantastic", "satisfied", "impressed", "happy" };
            var negativeKeywords = new List<string> { "bad", "terrible", "awful", "poor", "disappointed", "overripe", "rotten", "artificial", "stale", "unimpressed" };

            // Convert message to lowercase for case-insensitive matching
            message = message.ToLower();

            // Check if message contains any positive or negative keywords
            if (positiveKeywords.Any(keyword => message.Contains(keyword)))
            {
                return "Positive";
            }
            else if (negativeKeywords.Any(keyword => message.Contains(keyword)))
            {
                return "Negative";
            }
            else
            {
                return "Neutral";
            }
        }




        [HttpGet]
        public IActionResult Notification()
        {


            var paidCheckoutCount = unitOfWorks.Checkout.GetAll().Count(c => c.PaymentStatus == "Paid");

            // Get the count of pending checkouts
            var pendingCheckoutCount = unitOfWorks.Checkout.GetAll().Count(c => c.PaymentStatus == "Pending");

            var faildCheckoutCount = unitOfWorks.Checkout.GetAll().Count(c => c.PaymentStatus == "Failed");

            // Pass the counts to ViewBag or ViewData to be used in the view
            ViewBag.PaidCheckoutCount = paidCheckoutCount;
            ViewBag.PendingCheckoutCount = pendingCheckoutCount;
            ViewBag.FaildCheckoutCount = faildCheckoutCount;



            var feedback = unitOfWorks.Feedback.GetAll().Count();
            var contact = unitOfWorks.Contact.GetAll().Count();
            var enquiry = unitOfWorks.Enquiry.GetAll().Count();

            ViewBag.Contact = contact;
            ViewBag.Enquiry = enquiry;
            ViewBag.Feedback = feedback;

            SetAdminData();

            return View();
        }


        [HttpGet]
        public IActionResult EditCheckout(int id)
        {

            var user = unitOfWorks.Users.GetAll().ToList();

            var checkout = unitOfWorks.Checkout.GetById(id);

            SetAdminData();

            var model = new Tuple<Checkout, List<User>>(checkout, user);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditCheckout(Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                var checkoutInDb = unitOfWorks.Checkout.GetById(checkout.id);

                checkoutInDb.PaymentStatus = checkout.PaymentStatus;


                unitOfWorks.Checkout.Update(checkoutInDb);
                unitOfWorks.Save();
                TempData["Message"] = "Checkout Updated successfully!";
                return RedirectToAction("ViewCheckouts");
            }
            else
            {
                TempData["Error"] = "Checkout Not Updated !!!";
                return RedirectToAction("ViewCheckouts");
            }
        }




        [HttpGet]
        public IActionResult AddAsPositiveFeedback(int id)
        {
            var feedback = unitOfWorks.Feedback.GetById(id);
            var positiveFeedback = new PositiveFeedback
            {
                Name = feedback.Name,
                Email = feedback.Email,
                Phone = feedback.Phone,
                Message = feedback.Message,
                Date = DateTime.Now,
                MessageType = "Positive"
            };

            unitOfWorks.PositiveFeedback.Add(positiveFeedback);
            unitOfWorks.Save();

            TempData["Message"] = "Feedback Added as Positive Feedback!";
            return RedirectToAction("Feedback");
        }

        [HttpGet]
        public IActionResult PositiveFeedbacks()
        {
            var positiveFeedbacks = unitOfWorks.PositiveFeedback.GetAll().OrderByDescending(c => c.Id).ToList();
            SetAdminData();

            return View(positiveFeedbacks);
        }

        [HttpGet]
        public IActionResult DeletePositiveFeedback(int id)
        {
            var positiveFeedback = unitOfWorks.PositiveFeedback.GetById(id);

            unitOfWorks.PositiveFeedback.Remove(positiveFeedback);
            unitOfWorks.Save();

            TempData["Message"] = "Positive Feedback deleted successfully!";
            return RedirectToAction("PositiveFeedbacks");
        }





    }
}




