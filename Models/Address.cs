namespace HomePageBackend.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string? AddressLine { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public bool IsDefault { get; set; } = false;
        
        public User User { get; set; } = null!;
        public City City { get; set; } = null!;
        public State State { get; set; } = null!;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}