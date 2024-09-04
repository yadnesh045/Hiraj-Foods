using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IEnquiry : IRepository<Enquiry>
    {

        Enquiry GetByEmail(string email);

        void Update(Enquiry obj);
    }
}
