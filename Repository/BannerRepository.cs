using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class BannerRepository : Repository<Banner>, IBannerRepository
    {
        private ApplicationDbContext _db;
        public BannerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Banner GetByEmail(string email)
        {
            return _db.Banners.FirstOrDefault(u => u.Flavour_title == email);
        }

		public Banner GetById(int id)
		{
            return _db.Banners.FirstOrDefault(u => u.id == id);
		}

		public void Update(Banner obj)
        {
            _db.Banners.Update(obj);
        }
    }
}