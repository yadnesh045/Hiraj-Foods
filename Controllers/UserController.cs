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

	

		}

    }

