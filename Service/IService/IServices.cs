


namespace Hiraj_Foods.Services.IServices
{
    public interface IServices
    {
        string GenerateRandomInvoiceNumber();
        string SendForgetPassword(string email);
        bool SendLoginCredentials(string email, string password);
        bool SendOrderConfirmation(string email, string productsAndQuantities, decimal total);

    }
}
