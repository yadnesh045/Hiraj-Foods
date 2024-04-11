using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface ICheckoutRepository : IRepository<Checkout>
    {
        void Update(Checkout obj);
    }
}