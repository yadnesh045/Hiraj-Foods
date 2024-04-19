using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class FeedbackRepository : Repository<FeedBack>, IFeedBackRepository
    {

        private ApplicationDbContext _db;
        public FeedbackRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public FeedBack GetById(int id)
        {
            return _db.FeedBacks.Find(id);
        }

        public void Update(FeedBack obj)
        {
            _db.FeedBacks.Update(obj);
        }
    }
}