using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IBannerRepository : IRepository<Banner>
    {
        Banner GetById(int id);
        Banner GetByEmail(string email);

        void Update(Banner obj);
    }
}
