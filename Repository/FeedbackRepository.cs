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

        public FeedBack GetByEmail(string email)
        {
            return _db.FeedBacks.FirstOrDefault(u => u.Email == email);
        }

        public void Update(FeedBack obj)
        {
            _db.FeedBacks.Update(obj);
        }
    }
}