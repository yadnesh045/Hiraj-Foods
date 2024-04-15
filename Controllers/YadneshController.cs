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

            if(HttpContext.Session.GetInt32("UserId")!=null)
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
            return View(product);
        }


        public IActionResult MoreInfo(string name)
        {

            var product = unitOfWorks.Product.GetByFlavourName(name);
            return View(product);
        }


        public IActionResult Checkout()
        {
            return View();
        }



        [HttpPost]
        public IActionResult Checkout(Checkout checkout)
        {
            var userid = HttpContext.Session.GetInt32("UserId");
            var user = unitOfWorks.Users.GetById(userid);

            var orderTotal = unitOfWorks.Price.GetTotalPriceForUser(user.Id);


            var Chec = new Checkout
            {
                UserId = user.Id,
                Country = checkout.Country,
                City = checkout.City,
                Address1 = checkout.Address1,
                Address2 = checkout.Address2,
                paymentMethod = checkout.paymentMethod,
                pincode = checkout.pincode,
                Total = orderTotal.Price
            };

            unitOfWorks.Checkout.Add(Chec);
            unitOfWorks.Save();


            return RedirectToAction("Home" , "Yadnesh");
        }


        public void SetLayoutModel()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var user = unitOfWorks.Users.GetById(userId);
            var cartItems = unitOfWorks.Cart.GetByUserId(userId);
            var layoutModel = new LayoutModel { CartItemCount = cartItems.Count(), FirstName = user.FirstName, LastName = user.LastName };
            _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;
        }



        [HttpPost]
        public IActionResult SaveTotal(decimal total)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var existingTotal = unitOfWorks.Price.GetTotalPriceForUser(userId.Value);

       
                if (existingTotal != null)
                {
                    existingTotal.Price += total;
                }
                else
                {
                    existingTotal = new TotalPrice
                    {
                        UserId = userId.Value,
                        Price = total
                    };
                    unitOfWorks.Price.Add(existingTotal);
                }

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
