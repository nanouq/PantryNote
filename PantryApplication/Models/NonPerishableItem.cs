using System.ComponentModel.DataAnnotations;

namespace PantryApplication.Models
{
    public class NonPerishableItem : Item
    {
        [Required]
        public bool IsSealed { get; set; }
    }
}
