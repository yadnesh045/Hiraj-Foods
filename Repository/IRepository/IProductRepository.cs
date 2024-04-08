using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetById(int id);
        void Update(Product obj);
    }
}
