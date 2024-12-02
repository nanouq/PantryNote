using System.ComponentModel.DataAnnotations;

namespace PantryApplication.Models
{
    public class NonPerishableItem : Item
    {
        [Required]
        public bool IsSealed { get; set; }

        public override string IsExpired()
        {
            if(ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now && !IsSealed)
            {
                return $"{Name} is open and past expiration date. This item may be expired. Check for signs of spoilage before consuming.";
            }
            else if (ExpirationDate.HasValue && ExpirationDate.Value < DateTime.Now && IsSealed)
            {
                return $"{Name} is past expiration date, but still sealed. This item should be safe to eat, but check for signs of spoilage before consuming."; 
            }
            else
            {
                return $"{Name} is not expired.";
            }
            
        }
    }
}
