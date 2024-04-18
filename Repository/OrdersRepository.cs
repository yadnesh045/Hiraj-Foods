using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Repository
{
    internal class OrdersRepository : Repository<Orders>,IOrdersRepository
    {
        private ApplicationDbContext _db;
        public OrdersRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Orders> GetAllByUserId(int userId)
        {
            return _db.Uorders.Where(o => o.UserId == userId);
        }

        public Orders GetById(int orderId)
        {
            return _db.Uorders.FirstOrDefault(o => o.Id == orderId);
        }

        public Orders GetByUserId(int userId)
        {
            return _db.Uorders.FirstOrDefault(c => c.UserId == userId);
        }

        public void Update(Cart obj)
        {
            _db.Cart.Update(obj);
        }
    }
}