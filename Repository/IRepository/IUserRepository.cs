using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
	public interface IUserRepository: IRepository<User>	
	{
		User GetByEmail(string email);
        User GetById(int? id);

		User GetByPhone(string phone);
        void Update(User obj);

	}
}
