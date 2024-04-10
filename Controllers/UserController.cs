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

	

		}

    }

