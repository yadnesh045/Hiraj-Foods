using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IOrdersRepository : IRepository<Orders>
    {
        Orders GetByUserId(int userId);

        IEnumerable<Orders> GetAllByUserId(int userId);
        void Update(Cart obj);
    }
}
