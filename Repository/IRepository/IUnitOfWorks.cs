namespace Hiraj_Foods.Repository.IRepository
{
    public interface IUnitOfWorks
    {

        IAdminRepository Admin { get; }

        IProductRepository Product { get; }
    }
}
