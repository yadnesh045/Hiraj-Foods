﻿using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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
            SetLayoutModel();

            return View();
        }

        public IActionResult Quality_Values()
        {
            SetLayoutModel();

            return View();
        }

		public IActionResult Home()
		{
            SetLayoutModel();

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


            var Chec = new Checkout
            {
                UserId = user.Id,
                Country = checkout.Country,
                City = checkout.City,
                Address1 = checkout.Address1,
                Address2 = checkout.Address2,
                paymentMethod = checkout.paymentMethod,
                pincode = checkout.pincode
            };

            unitOfWorks.Checkout.Add(Chec);
            unitOfWorks.Save();


            return RedirectToAction("Home" , "Yadnesh");
        }


        public void SetLayoutModel()
        {
            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;
            var cartItems = unitOfWorks.Cart.GetByUserId(userId);
            var layoutModel = new LayoutModel { CartItemCount = cartItems.Count() };
            _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;
        }
    }
}
