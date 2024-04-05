﻿using Hiraj_Foods.Data;
using Hiraj_Foods.Repository.IRepository;

namespace Hiraj_Foods.Repository
{
    public class UnitOfWorks : IUnitOfWorks
    {

        private readonly ApplicationDbContext _db;
        public IAdminRepository Admin { get; set; }
        public UnitOfWorks(ApplicationDbContext _db)
        {
            this._db = _db;
            Admin = new AdminRepository(_db);

        }
  
    }
}
