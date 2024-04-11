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

			var existingUser = unitOfWorks.users.GetByEmail(usr.User.Email);
			if (existingUser != null)
			{
				// If the email already exists, return a view with an error message
				TempData["ErrorMessage"] = "Email is already registered.";
				return View("Signup");
			}

			if (usr.User != null)
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
					TempData["success"] = "Login Successful";
					return RedirectToAction("Home", "Yadnesh");
				}
				else
				{
					TempData["UnSuccess"] = "Invalid creadentials";
					return RedirectToAction("Signup", "User");
				}
			}
	}
    }

