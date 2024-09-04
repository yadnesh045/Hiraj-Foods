using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface ICheckoutRepository : IRepository<Checkout>
    {
        Checkout GetById(int id);
        void Update(Checkout obj);
    }
}