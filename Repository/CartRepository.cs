using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Repository
{


    internal class CartRepository : Repository<Cart>, ICartRepository
    {


        private ApplicationDbContext _db;

        public CartRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Cart GetById(int id)
        {
            return _db.Cart.Find(id);
        }

        public IEnumerable<Cart> GetByUserId(int userId)
        {
            return _db.Cart.Where(c => c.UserId == userId).ToList();
        }

        public Cart GetByUserIdAndProductId(int userId, int productId)
        {
            return _db.Cart.FirstOrDefault(c => c.UserId == userId && c.ProductId == productId);
        }

        public Cart GetByUserIdAndProductName(int id, string productName)
        {
            return _db.Cart.FirstOrDefault(c => c.UserId == id && c.ProductName == productName);    
        }

        public void Update(Cart obj)
        {
            _db.Cart.Update(obj);
        }
    }
}