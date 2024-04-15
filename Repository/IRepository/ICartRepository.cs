﻿using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Cart GetById(int id);
        IEnumerable<Cart> GetByUserId(int userId);
        Cart GetByUserIdAndProductId(int userId, int productId);
        Cart GetByUserIdAndProductName(int id, string productName);
        void Update(Cart obj);
    }
}