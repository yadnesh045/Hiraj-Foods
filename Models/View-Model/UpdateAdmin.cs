namespace Hiraj_Foods.Models.View_Model
{
    public class UpdateAdmin
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }

        public string? State { get; set; }
        public string? ZipCode { get; set; }
    }
}
