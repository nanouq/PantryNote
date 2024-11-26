using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryApplication.Models
{
    public class Pantry
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [ValidateNever]
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
    }
}
