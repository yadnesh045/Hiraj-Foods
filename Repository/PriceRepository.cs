using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Repository
{
    internal class PriceRepository : Repository<TotalPrice>, IPriceRepository
    {
        private ApplicationDbContext _db;
        public PriceRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(TotalPrice obj)
        {
            _db.Price.Update(obj);
        }

        public TotalPrice GetTotalPriceForUser(int userId)
        {
            return _db.Price.FirstOrDefault(tp => tp.UserId == userId);
        }
    }
}