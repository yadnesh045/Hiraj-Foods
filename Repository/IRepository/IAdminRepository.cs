using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IAdminRepository : IRepository<Admin>
    {
        Admin GetByEmail(string email);

        void Update(Admin obj);
    }
}
