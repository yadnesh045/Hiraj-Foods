using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class AdminRepository : Repository<Admin>, IAdminRepository
    {

        	
		private ApplicationDbContext _db;
        public AdminRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Admin GetByEmail(string email)
        {
            return _db.Admins.FirstOrDefault(u => u.Email == email);
        }

        public void Update(Admin obj)
        {
            _db.Admins.Update(obj);
        }
    }
}