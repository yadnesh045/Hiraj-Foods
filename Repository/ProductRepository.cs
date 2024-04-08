using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class ProductRepository : Repository<Product>, IProductRepository
    {

        private ApplicationDbContext _db;

       
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Product GetById(int id)
        {
            return _db.Products.Find(id);
        }


        public void Save(Product obj)
        {
            _db.Products.Add(obj);
        }

        public void Update(Product obj)
        {
            _db.Products.Update(obj);
        }
    }
}