namespace HomePageBackend.Models
{
    public class MenuCategory
    {
        public int CategoryId { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        
        public Restaurant Restaurant { get; set; } = null!;
        public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}