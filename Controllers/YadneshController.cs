using Hiraj_Foods.Migrations;
using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Hiraj_Foods.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Hiraj_Foods.Controllers
{
    public class YadneshController : Controller
    {

        private readonly IUnitOfWorks unitOfWorks;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServices _Service;

        public YadneshController(IUnitOfWorks unitOfWorks, IHttpContextAccessor httpContextAccessor, IServices services)
        {
            this.unitOfWorks = unitOfWorks;
            _httpContextAccessor = httpContextAccessor;
            _Service = services;    

        }
        public IActionResult Aboutus()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetLayoutModel();
            }

            return View();
        }

        public IActionResult Quality_Values()
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetLayoutModel();
            }

            return View();
        }

        public IActionResult Home()
        {
            // SetLayoutModel();

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                SetLayoutModel();
            }
            var products = unitOfWorks.Product.GetAll().OrderByDescending(p => p.Id).ToList();
            var banners = unitOfWorks.Banner.GetAll().ToList();

            var positiveFeedbacks = unitOfWorks.PositiveFeedback.GetAll().ToList();


            var model = new Tuple<List<Product>, List<Banner>, List<PositiveFeedback>>(products, banners, positiveFeedbacks);
            return View(model);
        }

        public IActionResult HomeInside(int id)
        {

            var product = unitOfWorks.Product.GetById(id);

            SetLayoutModel();
            return View(product);
        }


        public IActionResult MoreInfo(string name)
        {
            SetLayoutModel();
            var product = unitOfWorks.Product.GetName(name);
            return View(product);
        }


        public IActionResult Checkout()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Checkout(Checkout checkout, Product product)
        {

            var TID = HttpContext.Session.GetString("TranctionId");

 


            if(checkout.City != "Nashik" && checkout.paymentMethod == "CashOnDelivery")
            {
                TempData["Message"] = "COD is Not Avalilable Outside Nashik";
                return View();
            }


            if (checkout.TranscationID != "CashOnDelivery")
            {
                if (checkout.TranscationID != TID)
                {
                    TempData["Error"] = "Transction Id Did Not matched";
                    return View();
                }
            }


            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove("TransactionId");

            if (checkout != null)
            {
                var userid = HttpContext.Session.GetInt32("UserId");


                var user = unitOfWorks.Users.GetById(userid);
                    
                var cartItems = unitOfWorks.Cart.GetByUserId(user.Id);

                var productsAndQuantities = string.Join(", ", cartItems.Select(c => $"{c.ProductName}\t:{c.Quantity}\t:{c.ProductPrice}"));

                var productInDb = unitOfWorks.Product.GetById(product.Id);


                decimal total;

                if (productInDb == null && cartItems.Any())
                {
                    total = cartItems.Sum(c => c.Quantity * decimal.Parse(c.ProductPrice));
                }
                else if (product != null)
                {
                    if (!string.IsNullOrEmpty(productInDb.ProductPrice))
                    {
                        total = decimal.Parse(productInDb.ProductPrice); // Parse the string to decimal
                        productsAndQuantities = $"{productInDb.ProductName}\t\t:1\t:{productInDb.ProductPrice}";

                    }
                    else
                    {
                        return BadRequest("Product price is not available");
                    }
                }
                else
                {
                    return BadRequest("No product to checkout");
                }
                var paymentSatus = "";
                if (checkout.paymentMethod == "CashOnDelivery")
                {
                    paymentSatus = "Pending";
                }
                else
                {
                    paymentSatus = "Paid";
                }

            


                var Chec = new Checkout
                {
                    UserId = user.Id,
                    Country = checkout.Country,
                    City = checkout.City,
                    Address1 = checkout.Address1,
                    Address2 = checkout.Address2,
                    paymentMethod = checkout.paymentMethod,
                    ProductsAndQuantity = productsAndQuantities,
                    pincode = checkout.pincode,
                    Total = total,
                    Date = DateTime.Now,
                    PaymentStatus = paymentSatus,
                    TranscationID = checkout.TranscationID
                };
                    

                unitOfWorks.Checkout.Add(Chec);


                if (productInDb == null)
                {
                    foreach (var item in cartItems)
                    {
                        unitOfWorks.Cart.Remove(item);
                    }
                }

                ViewBag.Price = total;
                TempData["Success"] = "Order Placed Successfully";


                if (productInDb != null)
                {
                    var order = new Orders
                    {
                        UserId = user.Id,
                        Products = productsAndQuantities,
                        date = DateTime.Now,
                        Total = total,
                        Paymentmethod = checkout.paymentMethod,
                        status = "Order Recived",
                        ProductImageUrl = productInDb.ProductImageUrl
                    };

                    unitOfWorks.Uorders.Add(order);

                }
                else
                {
                    var order = new Orders
                    {
                        UserId = user.Id,
                        Products = productsAndQuantities,
                        date = DateTime.Now,
                        Total = total,
                        Paymentmethod = checkout.paymentMethod,
                        status = "Order Recived",
                        ProductImageUrl = "/All Products.jpg"
                    };

                    unitOfWorks.Uorders.Add(order);
                }
                unitOfWorks.Save();

                _Service.SendOrderConfirmation(user.Email, productsAndQuantities, total);

                return RedirectToAction("Home", "Yadnesh");
            }
            return View("Checkout");
        }





        public void SetLayoutModel()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId != 0)
            {

                var user = unitOfWorks.Users.GetById(userId);
                var cartItems = unitOfWorks.Cart.GetByUserId(userId);
                var Profilepic = unitOfWorks.UserImage.GetByUserId(userId);

                var layoutModel = new LayoutModel
                {
                    CartItemCount = cartItems.Count(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    profilepic = Profilepic?.user_Profile_Img // Use the null-conditional operator to avoid NullReferenceException
                };
                // If Profilepic is null, set a default image or leave it as null
                if (Profilepic == null)
                {
                    layoutModel.profilepic = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_1280.png"; // Or set a default image path
                }
                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;

            }
        }



        [HttpPost]
        public IActionResult SaveTotal(decimal total, string products)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            int intValue = (int)total;


            HttpContext.Session.SetInt32("productPrice", intValue);


            var userid = HttpContext.Session.GetInt32("UserId");
            var user = unitOfWorks.Users.GetById(userid);



            // Split the products string into an array of product details
            var productDetails = products.Split(", ");

            foreach (var detail in productDetails)
            {
                // Split each detail into product name and quantity
                var parts = detail.Split(":");
                var productName = parts[0];
                var quantity = int.Parse(parts[1]);

                // Find the product in the user's cart and update the quantity
                var cartItem = unitOfWorks.Cart.GetByUserIdAndProductName(user.Id, productName);
                cartItem.Quantity = quantity;
                unitOfWorks.Cart.Update(cartItem);
            }


            unitOfWorks.Save();



            if (userId.HasValue)
            {
                //var existingTotal = unitOfWorks.Price.GetTotalPriceForUser(userId.Value);
                //if (existingTotal != null)
                //{
                //    existingTotal.Price += total;
                //}
                //else
                //{
                //    existingTotal = new TotalPrice
                //    {
                //        UserId = userId.Value,
                //        Price = total
                //    };
                //    unitOfWorks.Price.Add(existingTotal);
                //}
                //unitOfWorks.Save();
                //return Ok();



                // Always create a new TotalPrice object and add it to the Price table
                var newTotal = new TotalPrice
                {
                    UserId = userId.Value,
                    Price = total
                };
                unitOfWorks.Price.Add(newTotal);

                unitOfWorks.Save();

                return Ok();

            }
            else
            {
                return BadRequest("User ID is not available.");
            }
        }

        [HttpPost]
        public IActionResult SetProductPriceFormCart(decimal total)
        {
            int intValue = (int)total;
            HttpContext.Session.SetInt32("productPrice", intValue);
            return Ok();
        }






    }
}
