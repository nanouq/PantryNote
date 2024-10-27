using System.ComponentModel.DataAnnotations;

namespace PantryApplication.Models
{
    public class Pantry
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
    }
}
