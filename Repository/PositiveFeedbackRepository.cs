using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    public class PositiveFeedbackRepository : Repository<PositiveFeedback>, IPositiveFeedbackRepository
    {
        private ApplicationDbContext _db;

        public PositiveFeedbackRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public PositiveFeedback GetById(int id)
        {
            return _db.PositiveFeedbacks.Find(id);
        }

        public void Update(PositiveFeedback obj)
        {
            _db.PositiveFeedbacks.Update(obj);
        }
    }
}
