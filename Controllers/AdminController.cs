using Hiraj_Foods.Models.View_Model;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class AdminController : Controller
    {

        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(LoginData Vm)
        {
            return View();
        }

        public IActionResult dashboard()
        {
            return View();
        }


    }
}
