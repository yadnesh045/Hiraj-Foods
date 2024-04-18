using Hiraj_Foods.Models;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Data
{
    public class ApplicationDbContext : DbContext
    {


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }
        public DbSet<Banner> Banners { get; set; }

        public DbSet<Cart> Cart { get; set; }

        public DbSet<Checkout> Checkout { get; set; }
        public DbSet<TotalPrice> Price { get; set; }


        public DbSet<User> Users { get; set; }

        public DbSet<Orders> Uorders { get; set; }

        public DbSet<UserProfileImg> UserProfileImage { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1, Email = "Admin@gmail.com", FirstName = "Admin", LastName="HighTech", Mobile = "8668212142", Address = "Nashik", Password = "Admin@123" }
            );


        }

    }
}
