namespace PantryApplication.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime DateAdded { get; set; }
        public int Quantity { get; set; }
        public string Unit { get; set; }
        public string Location { get; set; }
        public int PantryId { get; set; }
    }
}
