using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IFeedBackRepository : IRepository<FeedBack>
    {
        FeedBack GetById(int id);


        void Update(FeedBack obj);
    }
}