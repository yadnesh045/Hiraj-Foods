using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IFeedBackRepository : IRepository<FeedBack>
    {
        FeedBack GetByEmail(string email);

        void Update(FeedBack obj);
    }
}