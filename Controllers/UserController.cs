using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

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

    }

}

