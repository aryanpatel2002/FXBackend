namespace HomePageBackend.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Address { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        public User User { get; set; } = null!;
        public City City { get; set; } = null!;
        public State State { get; set; } = null!;
        public ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}