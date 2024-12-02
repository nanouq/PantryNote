using System.ComponentModel.DataAnnotations;

namespace PantryApplication.Models
{
    public class PerishableItem : Item
    {
        [Required]
        public bool RequiresRefrigeration { get; set; }
        [Required]
        public bool RequiresFreezing { get; set; }

        public override string IsExpired()
        {
            if (ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now)
            {
                return $"{Name} is past expiration and could be expired. Check for signs of spoilage before consuming.";
            }
            else
            {
                return $"{Name} is not expired.";
            }
        }

        public override string GetStorageInstructions()
        {
            if (RequiresRefrigeration && RequiresFreezing)
            {
                return $"Store {Name} in either the refrigerator or freezer depending on usage.";
            }
            else if (RequiresFreezing)
            {
                return $"Store {Name} in the freezer for optimal freshness.";
            }
            else if (RequiresRefrigeration) 
            {
                return $"Store {Name} in the refrigerator for optimal freshness.";
            }
            else
            {
                return base.GetStorageInstructions();
            }           
        }
    }
}
