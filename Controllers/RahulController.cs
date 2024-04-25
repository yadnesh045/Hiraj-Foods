using Hiraj_Foods.Models;
using Hiraj_Foods.Models.View_Model;
using Hiraj_Foods.Repository;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;

namespace Hiraj_Foods.Controllers
{
    public class RahulController : Controller
    {
        private readonly IUnitOfWorks unitOfWorks;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public RahulController(IUnitOfWorks unitOfWorks, IHttpContextAccessor httpContextAccessor)
        {
            this.unitOfWorks = unitOfWorks;
            _httpContextAccessor = httpContextAccessor;

        }

        public IActionResult Contact()
        {
            SetLayoutModel();

            return View();
        }


        [HttpPost]
        public IActionResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                contact.Date = DateTime.Now;
                unitOfWorks.Contact.Update(contact);
                unitOfWorks.Save();
            }
            else
            {
                Console.WriteLine("-----------------------------------------------");
                return View();
            }

            TempData["Contact"] = "Contact Sent to Hiraj Foods";
            return RedirectToAction("Contact", "Rahul");
        }

        public IActionResult Feedback()
        {
            SetLayoutModel();

            return View();
        }

        [HttpPost]
        public IActionResult Feedback(FeedBack Enq)
        {

            if (ModelState.IsValid)
            {
                Enq.Date = DateTime.Now;
                unitOfWorks.Feedback.Add(Enq);
                unitOfWorks.Save();
            }
            else
            {
                Console.WriteLine("-----------------------------------------------");
                return View();
            }

            TempData["Feedback"] = "Feedback Sent to Hiraj Foods";
            return RedirectToAction("Feedback", "Rahul");
        }

        public IActionResult Enquiry()
        {
            SetLayoutModel();

            return View();
        }

        [HttpPost]
        public IActionResult Enquiry(Enquiry Enq)
        {

            if (ModelState.IsValid)
            {
                Enq.Date = DateTime.Now;
                unitOfWorks.Enquiry.Add(Enq);
                unitOfWorks.Save();
            }
            else
            {
                Console.WriteLine("-----------------------------------------------");
                return View();
            }

            TempData["Enquiry"] = "Enquiry Sent to Hiraj Foods";
            return RedirectToAction("Enquiry", "Rahul");
        }



        public void SetLayoutModel()
        {

            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId != 0)
            {

                var user = unitOfWorks.Users.GetById(userId);
                var cartItems = unitOfWorks.Cart.GetByUserId(userId);
                var Profilepic = unitOfWorks.UserImage.GetByUserId(userId);

                var layoutModel = new LayoutModel
                {
                    CartItemCount = cartItems.Count(),
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    profilepic = Profilepic?.user_Profile_Img // Use the null-conditional operator to avoid NullReferenceException
                };
                // If Profilepic is null, set a default image or leave it as null
                if (Profilepic == null)
                {
                    layoutModel.profilepic = "\"~/img/avatars/2.png\""; // Or set a default image path
                }

                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;

            }

        }
    }
}
