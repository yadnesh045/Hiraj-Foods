using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Hiraj_Foods.Controllers
{
	public class UserController : Controller
	{
		private readonly IUnitOfWorks unitOfWorks;
		private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUnitOfWorks unitOfWorks, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment _webHostEnvironment)
		{
			this.unitOfWorks = unitOfWorks;
			_httpContextAccessor = httpContextAccessor;
            this._webHostEnvironment = _webHostEnvironment;
        }

		public IActionResult Profile()
		{
			var userEmail = HttpContext.Session.GetString("UserEmail");
			SetLayoutModel();

			if (userEmail == null)
			{
				return RedirectToAction("Login", "Signup");
			}

			var user = unitOfWorks.Users.GetByEmail(userEmail);
			var UserProfile = unitOfWorks.UserImage.GetByUserId(user.Id);

            var model = new Tuple<User, UserProfileImg>(user, UserProfile);

            return View(model);
		}


		[HttpGet]
		public IActionResult Cart()
		{
			int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
			var cartItems = unitOfWorks.Cart.GetByUserId(userId);
			SetLayoutModel();
			return View(cartItems);
		}

		[HttpGet]
		public IActionResult AddCart(int id)
		{
			int? userId = HttpContext.Session.GetInt32("UserId");
			var product = unitOfWorks.Product.GetById(id);

			if (userId is not null)
			{
				var user = unitOfWorks.Users.GetById(userId.Value);
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

			if (cartitem is not null)
			{
				unitOfWorks.Cart.Remove(cartitem);
				unitOfWorks.Save();
				TempData["Success"] = "Item Removed From Cart";
			}
			else
			{
				TempData["Error"] = "Item Not Removed";
			}

			return RedirectToAction("Cart", "User");
		}

		public void SetLayoutModel()
		{
			int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
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

            TempData["Message"] = "Profile Updated successfully!";
            return RedirectToAction("Profile");

        }


        [HttpPost]
        public IActionResult EditUserPhoto(UserProfileImg usr, IFormFile File)
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = unitOfWorks.Users.GetById(userId);

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (File != null)
                {
                    // Delete old image if exists
                    var oldProfileImg = unitOfWorks.UserImage.GetByUserId(userId);
                    if (oldProfileImg != null)
                    {
                        string oldImagePath = Path.Combine(wwwRootPath, oldProfileImg.user_Profile_Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }


                        unitOfWorks.UserImage.Delete(oldProfileImg.Id);
                    }

                    // Save new image
                    string filename = Guid.NewGuid().ToString() + Path.GetExtension(File.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"UserProfilePic");
                    using (var fileStream = new FileStream(Path.Combine(productPath, filename), FileMode.Create))
                    {
                        File.CopyTo(fileStream);
                    }

                    var newProfileImg = new UserProfileImg
                    {
                        UserId = user.Id,
                        user_Profile_Img = @"\UserProfilePic\" + filename
                    };
                    unitOfWorks.UserImage.Update(newProfileImg);
                    unitOfWorks.Save();

                    TempData["Message"] = "Profile updated successfully!";
                    return RedirectToAction("Profile");
                }
            }

            TempData["Error"] = "Profile not updated!";
            return RedirectToAction("Profile");
        }

    }


}
