using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IUserProfileImgRepository : IRepository<UserProfileImg>
    {

        void Update(UserProfileImg obj);
        UserProfileImg GetByUserId(int userId);

        void Delete(int userProfileImgId);
    }
}