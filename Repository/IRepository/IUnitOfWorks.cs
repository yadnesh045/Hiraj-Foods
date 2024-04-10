namespace Hiraj_Foods.Repository.IRepository
{
    public interface IUnitOfWorks
    {

        IAdminRepository Admin { get; }

        IProductRepository Product { get; }


        IContactRepository Contact { get; }

        IUserRepository Users { get; }    

        IEnquiry Enquiry { get; }

        IFeedBackRepository Feedback { get; }

        IBannerRepository Banner { get; }
        void Save();
    }
}
