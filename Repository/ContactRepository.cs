using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Repository
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        private readonly ApplicationDbContext _db; 
        public ContactRepository(ApplicationDbContext db) : base(db)
        {
            _db= db;    
        }
        public Contact GetByEmail(string email)
        {
            return _db.Contacts.FirstOrDefault(u => u.Email == email);
        }

        public void Update(Contact obj)
        {
            _db.Contacts.Update(obj);
        }

        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
