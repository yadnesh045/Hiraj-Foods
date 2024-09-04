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

        public Product GetByFlavourName(string name)
        {
            return _db.Products.FirstOrDefault(u => u.ProductName == name);
        }

        public Product GetById(int id)
        {
            return _db.Products.Find(id);
        }

        public Product GetName(string name)
        {
            return _db.Products.FirstOrDefault(u => u.ProductFlavour == name);
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