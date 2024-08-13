using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hiraj_Foods.Models;
using Newtonsoft.Json;
using Hiraj_Foods.Services.IServices;
using System.Web.Helpers;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace Hiraj_Foods.Controllers
{

    public class LoginController : Controller
    {

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWorks unitOfWorks;
        private readonly IServices _Service;


        public LoginController(IUnitOfWorks unitOfWorks, IWebHostEnvironment _webHostEnvironment, IServices services)
        {
            this.unitOfWorks = unitOfWorks;
            this._webHostEnvironment = _webHostEnvironment;
            _Service = services;
        }



        // for admin login
        [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Login()
        {
            // if user is login then show tempdata message that he is not authorized to login
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                TempData["Error"] = "Not Authorized to Login!!!";
                return RedirectToAction("Home", "Yadnesh");
            }



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

                if (Admin != null)
                {
                    // just check if admin is active or not
                    if (Admin.IsActive == false)
                    {
                        TempData["Error"] = "Not Authorized to Login!!!";

                        return RedirectToAction("Login", "Login");
                    }
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

                    TempData["Success"] = "Login Successfull !";

                    return RedirectToAction("dashboard", "Admin");
                }

                else
                {
                    TempData["Error"] = "Invalid Crendentails";
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
            TempData["Success"] = "Logout Successfull !";

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
                // Check if the Email is already exists
                var existingEmailUser = unitOfWorks.Users.GetByEmail(usr.User.Email);
                if (existingEmailUser != null)
                {
                    TempData["Error"] = "Email is Already Exists";
                    //return RedirectToAction("Signup", "Login");
                }

                // Check if the phone number already exists
                var existingPhoneUser = unitOfWorks.Users.GetByPhone(usr.User.Phone);
                if (existingPhoneUser != null)
                {
                    TempData["Error"] = "Phone no. is Already Exists";
                    return RedirectToAction("Signup", "Login");
                }

                //if (!ModelState.IsValid)
                //{
                //    return RedirectToAction("Signup", "Login");
                //}
                else
                {

                    usr.User.Password = Crypto.HashPassword(usr.User.Password);
                    unitOfWorks.Users.Add(usr.User);
                    unitOfWorks.Save();

                    var existinguser = unitOfWorks.Users.GetById(usr.User.Id);

                    if (existinguser != null)
                    {
                        HttpContext.Session.SetInt32("UserId", existinguser.Id);
                        HttpContext.Session.SetString("UserEmail", existinguser.Email);


                        _Service.SendLoginCredentials(existinguser.Email, existinguser.Password);

                        TempData["Success"] = "Account Created And Login Successfull.";
                        return RedirectToAction("Home", "Yadnesh");
                    }
                }
            }

            return RedirectToAction("Signup", "Login");
        }




        [HttpPost]
        public IActionResult UserLogin(User_SignIn_Login log)
        {
            if (HttpContext.Session.GetInt32("AdminId") != null)
            {
                TempData["Error"] = "Not Authorized to Login!!!";
                return RedirectToAction("SignUp", "Login");
            }


            var existingUser = unitOfWorks.Users.GetByEmail(log.Login.Email);
            // Crypto.VerifyHashedPassword(existingUser.Password, log.Login.Password);

            if (existingUser != null)
            {
                bool doesPasswordMatch = Crypto.VerifyHashedPassword(existingUser.Password, log.Login.Password);

                if (doesPasswordMatch)
                {
                    //set session for user -- store user id and email
                    HttpContext.Session.SetInt32("UserId", existingUser.Id);
                    HttpContext.Session.SetString("UserEmail", existingUser.Email);

                    TempData["Success"] = "Login Successfull.";
                    return RedirectToAction("Home", "Yadnesh");
                }
                else
                {
                    TempData["Error"] = "Invalid Credentials";
                    return View("Signup");
                }

            }
            else
            {
                TempData["Error"] = "Invalid Credentials";
                return View("Signup");
            }
        }


        [HttpGet]
        public async Task<IActionResult> UserLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            TempData["Success"] = "Logout Successfull !";

            return RedirectToAction("Home", "Yadnesh");
        }



        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }



        [HttpPost]
        public IActionResult ForgetPassword(User_SignIn_Login usr)
        {
            if (usr == null || usr.Login == null)
            {
                TempData["Error"] = "Invalid request.";
                return RedirectToAction("ForgetPassword", "Login");
            }

            var existingUser = unitOfWorks?.Users?.GetByEmail(usr.Login.Email);

            if (existingUser != null)
            {
                // random password must be generated and sent to the user email address in the service named sendforgetpassword
                string newPassword = _Service.SendForgetPassword(existingUser.Email);

                if (newPassword != null)
                {
                    existingUser.Password = Crypto.HashPassword(newPassword);
                    unitOfWorks.Users.Update(existingUser);
                    unitOfWorks.Save();

                    TempData["Success"] = "Password Sent to your Email.";
                    return RedirectToAction("Home", "Yadnesh");
                }
                else
                {
                    TempData["Error"] = "Error sending email.";
                    return RedirectToAction("ForgetPassword", "Login");
                }
            }
            else
            {
                TempData["Error"] = "Email Not Found.";
                return RedirectToAction("ForgetPassword", "Login");
            }
        }






    }
}
