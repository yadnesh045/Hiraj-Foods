using Hiraj_Foods.Data;
using Hiraj_Foods.Models;
using Hiraj_Foods.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Hiraj_Foods.Repository
{
    internal class UserProfileImgRepository : Repository<UserProfileImg>, IUserProfileImgRepository
    {
        private readonly ApplicationDbContext _db;
        public UserProfileImgRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public UserProfileImg GetByUserId(int userId)
        {
            return _db.UserProfileImage.FirstOrDefault(u => u.UserId == userId);
        }

        public void Update(UserProfileImg obj)
        {
            _db.UserProfileImage.Update(obj);
        }


        public void Delete(int userProfileImgId)
        {
            var userProfileImg = _db.UserProfileImage.Find(userProfileImgId);
            if (userProfileImg != null)
            {
                _db.UserProfileImage.Remove(userProfileImg);
                _db.SaveChanges();
            }
        }


    }
}