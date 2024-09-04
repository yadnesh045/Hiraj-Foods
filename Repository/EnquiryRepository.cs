using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    internal class EnquiryRepository : Repository<Enquiry>, IEnquiry
    {
        private ApplicationDbContext _db;
        public EnquiryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Enquiry GetByEmail(string email)
        {
            return _db.Enquiries.FirstOrDefault(u => u.Email == email);
        }

        public void Update(Enquiry obj)
        {
            _db.Enquiries.Update(obj);
        }
    }
}