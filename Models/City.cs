namespace HomePageBackend.Models
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; } = string.Empty;
        public int StateId { get; set; }
        public bool IsActive { get; set; } = true;
        
        public State State { get; set; } = null!;
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}