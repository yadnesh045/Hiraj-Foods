using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
	public class UserRepository : Repository<User>, IUserRepository
	{
		private readonly ApplicationDbContext _db;
		public UserRepository(ApplicationDbContext db) : base(db)
		{
			_db= db;	
		}

		public User GetByEmail(string email)
		{
			return _db.Users.FirstOrDefault(u => u.Email == email);
		}

		public void Update(User obj)
		{
			_db.Users.Update(obj);
		}
	}
}
