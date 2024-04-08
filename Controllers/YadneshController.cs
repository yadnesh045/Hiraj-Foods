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
            var banner = unitOfWorks.Banner.GetAll().ToList();
            return View(banner);
        }

        public IActionResult HomeInside()
        {
            return View();
        }
    }
}
