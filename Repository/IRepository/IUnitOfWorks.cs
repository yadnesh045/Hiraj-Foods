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

        ICartRepository Cart { get; }

        ICheckoutRepository Checkout { get; }

        IPriceRepository Price { get; }

        IUserProfileImgRepository UserImage { get; }
        void Save();
    }
}
