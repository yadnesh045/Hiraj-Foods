


namespace Hiraj_Foods.Services.IServices
{
    public interface IServices
    {
        bool SendLoginCredentials(string email, string password);
        bool SendOrderConfirmation(string email, string productsAndQuantities, decimal total);

    }
}
