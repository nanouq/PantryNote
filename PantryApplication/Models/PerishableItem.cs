using System.ComponentModel.DataAnnotations;

namespace PantryApplication.Models
{
    public class PerishableItem : Item
    {
        [Required]
        public bool RequiresRefrigeration { get; set; }
        [Required]
        public bool RequiresFreezing { get; set; }
    }
}
