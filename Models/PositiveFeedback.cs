using System.ComponentModel.DataAnnotations;

namespace Hiraj_Foods.Models
{
    public class PositiveFeedback
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Message { get; set; }
        public DateTime? Date { get; set; }

        public string MessageType { get; set; }
    }
}
