using Hiraj_Foods.Migrations;
using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Controllers
{
    public class YadneshController : Controller
    {

        private readonly IUnitOfWorks unitOfWorks;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public YadneshController(IUnitOfWorks unitOfWorks, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWorks = unitOfWorks;
            _httpContextAccessor = httpContextAccessor;

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

            var model = new Tuple<List<Product>, List<Banner>>(products, banners);
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
            var userid = HttpContext.Session.GetInt32("UserId");


            var user = unitOfWorks.Users.GetById(userid);

            var cartItems = unitOfWorks.Cart.GetByUserId(user.Id);
            var productsAndQuantities = string.Join(", ", cartItems.Select(c => $"{c.ProductName}:{c.Quantity}")); // Ensure Quantity is correctly retrieved

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
                    productsAndQuantities = $"{productInDb.ProductName}:1";

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
                PaymentStatus = paymentSatus
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

            unitOfWorks.Save();

            TempData["Success"] = "Order Placed Successfully";
            return RedirectToAction("Home", "Yadnesh");
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
                    layoutModel.profilepic = null; // Or set a default image path
                }
                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;

            }
        }



        [HttpPost]
        public IActionResult SaveTotal(decimal total, string products)
        {
            var userId = HttpContext.Session.GetInt32("UserId");



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

        


    }
}
