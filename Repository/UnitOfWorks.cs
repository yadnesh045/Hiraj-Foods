using Hiraj_Foods.Data;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    public class UnitOfWorks : IUnitOfWorks
    {

        private readonly ApplicationDbContext _db;
        public IAdminRepository Admin { get; set; }

        public IProductRepository Product { get; set; }

        public IEnquiry Enquiry { get; set; }

        public IFeedBackRepository Feedback { get; set; }



        public UnitOfWorks(ApplicationDbContext _db)
        {
            this._db = _db;
            Admin = new AdminRepository(_db);
            Product = new ProductRepository(_db);
            Enquiry = new EnquiryRepository(_db);
            Feedback = new FeedbackRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
