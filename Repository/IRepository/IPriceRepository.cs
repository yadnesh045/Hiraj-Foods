using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IPriceRepository: IRepository<TotalPrice>
    {
        void Update(TotalPrice obj);
        TotalPrice GetTotalPriceForUser(int userId);

    }
}