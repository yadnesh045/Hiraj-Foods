using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IContactRepository :IRepository<Contact>
    {
        Contact GetByEmail(string email);

        void Update(Contact obj);



    }
}
