using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hiraj_Foods.Models;
using Newtonsoft.Json;

namespace Hiraj_Foods.Controllers
{

	public class LoginController : Controller
	{

		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IUnitOfWorks unitOfWorks;


		public LoginController(IUnitOfWorks unitOfWorks, IWebHostEnvironment _webHostEnvironment)
		{
			this.unitOfWorks = unitOfWorks;
			this._webHostEnvironment = _webHostEnvironment;
		}



        // for admin login
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
		{
			return View();
		}


		[HttpPost]
		public async Task<IActionResult> Login(LoginData Vm)
		{

			if (Vm != null)
			{
				string enteredEmail = Vm.EnteredEmail;
				string enteredPassword = Vm.EnteredPassword;

				var Admin = unitOfWorks.Admin.GetByEmail(enteredEmail);

				// just check if admin is active or not
				if (Admin.IsActive == false)
				{
                    TempData["Error"] = "Not Authorized to Login!!!";

                    return RedirectToAction("Login", "Login");
                }

				if (Admin != null && Admin?.Password == enteredPassword)
				{
					//set session for admin store admin id and email
					HttpContext.Session.SetInt32("AdminId", Admin.Id);
					HttpContext.Session.SetString("AdminEmail", Admin.Email);

					// Sign in the admin
					var claims = new List<Claim>
					{
							new Claim(ClaimTypes.Name, Admin.Email)
					};

					var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

					var authProperties = new AuthenticationProperties();

					await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    TempData["Message"] = "Login Successfull !";

                    return RedirectToAction("dashboard", "Admin");
				}

				else
				{
                    TempData["Error"] = "Invalid Credentials";
                    return View();
				}

			}
			return View();
		}

		// for admin logout
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			HttpContext.Session.Clear();
			return RedirectToAction("Login", "Login");
		}



		// for user login

		public IActionResult Signup()
		{
			return View();
		}

		[HttpPost]
		public IActionResult UserReg(User_SignIn_Login usr)
		{


			if (usr != null)
			{
				unitOfWorks.Users.Add(usr.User);
				unitOfWorks.Save();

				return View("Signup");
			}


			return View();
		}




		[HttpPost]
		public IActionResult UserLogin(User_SignIn_Login log)
		{
			var existingUser = unitOfWorks.Users.GetByEmail(log.Login.Email);


			if (existingUser != null && existingUser.Password == log.Login.Password)
			{
				//set session for user -- store user id and email
				HttpContext.Session.SetInt32("UserId", existingUser.Id);
				HttpContext.Session.SetString("UserEmail", existingUser.Email);

				TempData["sucess"] = "Login Successfull !";

				return RedirectToAction("Home", "Yadnesh");
			}
			else
			{
				ModelState.AddModelError("", "Invalid email or password.");
				return View("Signup");
			}
		}


		[HttpGet]
		public async Task<IActionResult> UserLogout()
		{
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();

            return RedirectToAction("Home", "Yadnesh");
        }










	}
}
