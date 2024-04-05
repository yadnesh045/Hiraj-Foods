﻿using Hiraj_Foods.Models;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Data
{
    public class ApplicationDbContext: DbContext
    {

        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Admin> Admins { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Enquiry> Enquiries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>().HasData(
                new Admin { Id = 1, Email = "Admin@gmail.com", Name="Admin", Mobile="8668212142", Address="Nashik", Password = "Admin@123"}
            );
        }

        }
}
