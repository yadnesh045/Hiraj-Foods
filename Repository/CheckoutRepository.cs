using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class CheckoutRepository : Repository<Checkout>, ICheckoutRepository
    {

        private readonly ApplicationDbContext _db;
        public CheckoutRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Checkout GetById(int id)
        {
            return _db.Checkout.Find(id);
        }

        public void Update(Checkout obj)
        {
            _db.Checkout.Update(obj);
        }
    }
}