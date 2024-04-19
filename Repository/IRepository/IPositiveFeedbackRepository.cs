using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
    public interface IPositiveFeedbackRepository : IRepository<PositiveFeedback>
    {
        void Update(PositiveFeedback obj);

        PositiveFeedback GetById(int id);
       
    }
}
