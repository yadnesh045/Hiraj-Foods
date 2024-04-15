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

        public IActionResult Feedback()
        {
            SetLayoutModel();

            return View();
        }

        public IActionResult Enquiry()
        {
            SetLayoutModel();

            return View();
        }
        public void SetLayoutModel()
        {



            int userId = HttpContext.Session.GetInt32("UserId") ?? 0;

            if (userId != 0)
            {

                var user = unitOfWorks.Users.GetById(userId);
                var cartItems = unitOfWorks.Cart.GetByUserId(userId);
                var Profilepic = unitOfWorks.UserImage.GetByUserId(userId);

                var layoutModel = new LayoutModel { CartItemCount = cartItems.Count(), 
                    FirstName = user.FirstName, 
                    LastName = user.LastName, 
                    profilepic= Profilepic.user_Profile_Img 
                };

                _httpContextAccessor.HttpContext.Items["LayoutModel"] = layoutModel;

            }

        }
    }
}
