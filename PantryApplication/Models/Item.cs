using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PantryApplication.Models
{
    public abstract class Item
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string? Brand { get; set; }
        [Required]
        public string Category { get; set; }
        public DateTime? ExpirationDate { get; set; }
        [Required]
        public DateTime DateAdded { get; set; }
        [Required]
        [Range(typeof(decimal), "0","1000")]
        public decimal Quantity { get; set; }
        [Required]
        [MaxLength(20)]
        public string Unit { get; set; }
        [Required]
        [MaxLength(50)]
        public string Location { get; set; }
        [Required]
        public int PantryId { get; set; }
        [ForeignKey("PantryId")]
        [ValidateNever]
        public Pantry Pantry { get; set; }
    }
}
