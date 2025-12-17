namespace HomePageBackend.Models
{
    public class State
    {
        public int StateId { get; set; }
        public string StateName { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        public ICollection<City> Cities { get; set; } = new List<City>();
        public ICollection<Restaurant> Restaurants { get; set; } = new List<Restaurant>();
        public ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}