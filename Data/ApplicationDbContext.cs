using Hiraj_Foods.Models;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Anurag_Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }



    }
}
