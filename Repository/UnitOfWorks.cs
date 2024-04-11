using Hiraj_Foods.Data;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    public class UnitOfWorks : IUnitOfWorks
    {

        private readonly ApplicationDbContext _db;
        public IAdminRepository Admin { get; set; }
        public IProductRepository Product { get; set; }
        public IContactRepository Contact {get;set;}
		public IUserRepository users {  get; set; } 
        public IEnquiry Enquiry { get; set; }

        public IFeedBackRepository Feedback { get; set; }

        public IBannerRepository Banner { get; set; }

        public UnitOfWorks(ApplicationDbContext _db)

        {
            this._db = _db;
            Admin = new AdminRepository(_db);
            Product = new ProductRepository(_db);
            Contact = new ContactRepository(_db); 
            users = new UserRepository(_db);    
            Enquiry = new EnquiryRepository(_db);
            Feedback = new FeedbackRepository(_db);
            Banner = new BannerRepository(_db);

        }

        public void Save()
        {
            _db.SaveChanges();
        }
}
}
