namespace HomePageBackend.Models
{
    public class Review
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public int RestaurantId { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public User User { get; set; } = null!;
        public Restaurant Restaurant { get; set; } = null!;
    }
}