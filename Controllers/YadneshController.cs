using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class YadneshController : Controller
    {

		private readonly IUnitOfWorks unitOfWorks;

        public YadneshController(IUnitOfWorks unitOfWorks)
        {
			this.unitOfWorks = unitOfWorks;
		}
        public IActionResult Aboutus()
        {
            return View();
        }

        public IActionResult Quality_Values()
        {
            return View();
        }

		public IActionResult Home()
		{
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

     
    }
}
