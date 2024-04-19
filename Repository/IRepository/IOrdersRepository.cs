using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IOrdersRepository : IRepository<Orders>
    {
        Orders GetByUserId(int userId);

        Orders GetById(int orderId);
        IEnumerable<Orders> GetAllByUserId(int userId);
        void Update(Orders obj);
    }
}
