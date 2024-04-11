using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Hiraj_Foods.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWorks unitOfWorks;

        public UserController(IUnitOfWorks unitOfWorks)
        {
            this.unitOfWorks = unitOfWorks;
        }



        public IActionResult Profile()
        {
            // get user email from session
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (userEmail == null)
            {
                return RedirectToAction("Login", "Signup");

            }

            // get the data of the user by email
            var user = unitOfWorks.Users.GetByEmail(userEmail);


            return View(user);
        }

		public IActionResult Signup()
        {
            return View();
        }
		[HttpPost]
        public IActionResult UserReg(User_SignIn_Login usr)
        {
               
            if (usr.User!= null)
            {
                unitOfWorks.users.Add(usr.User);
                unitOfWorks.Save();

                return RedirectToAction("Home", "Yadnesh");

            }
			return View();
		}

		[HttpPost]
        public IActionResult UserLogin(User_SignIn_Login log)
        {
			var existingUser = unitOfWorks.users.GetByEmail(log.Login.Email);


			if (existingUser != null && existingUser.Password == log.Login.Password)
			{
			
				return RedirectToAction("Home", "Yadnesh");
			}
			else
			{
				TempData["Error"] = "Invalid Crendentails";
				return View("Signup");
			}
		}


        [HttpGet]
        public IActionResult Cart()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            var cartItems = unitOfWorks.Cart.GetByUserId(userId);

            ViewBag.CartItemCount = cartItems.Count();
            return View(cartItems);
        }

        [HttpGet]
        public IActionResult AddCart(int id)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var product = unitOfWorks.Product.GetById(id);

            if (userId is not null)
            {
                var user = unitOfWorks.users.GetById(userId);

                // Check if the product is already present in the user's cart
                var existingCartItem = unitOfWorks.Cart.GetByUserIdAndProductId(user.Id, product.Id);

                if (existingCartItem == null)
                {
                    var cart = new Cart
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        ProductName = product.ProductName,
                        ProductImageUrl = product.ProductImageUrl,
                        ProductDescription = product.ProductDescription,
                        ProductWeight = product.ProductWeight,
                        ProductPrice = product.ProductPrice
                    };

                    unitOfWorks.Cart.Add(cart);
                    unitOfWorks.Save();
                    TempData["Success"] = "Product added to cart.";
                }
                else
                {
                    TempData["Info"] = "Product is already in your cart.";
                }

                return RedirectToAction("HomeInside", "Yadnesh", new { id = product.Id });
            }
            TempData["Error"] = "You need to be logged in to add the product to the cart.";
            return RedirectToAction("HomeInside", "Yadnesh", new { id = product.Id });
        }


        [HttpGet]
        public IActionResult DeleteFromCart(int id)
        {
            var cartitem = unitOfWorks.Cart.GetById(id);

            if(cartitem is not null)
            {
                unitOfWorks.Cart.Remove(cartitem);
                unitOfWorks.Save();


                TempData["Success"] = "Item Removed From Cart";
                return RedirectToAction("Cart", "User");
            }

            TempData["Error"] = "Item Not Removed";
            return RedirectToAction("Cart", "User");
        }


    }

}

