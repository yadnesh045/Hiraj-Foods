using Hiraj_Foods.Models;

namespace Hiraj_Foods.Repository.IRepository
{
	public interface IUserRepository: IRepository<User>	
	{
		User GetByEmail(string email);

		void Update(User obj);

	}
}
