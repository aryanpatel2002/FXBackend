using Microsoft.EntityFrameworkCore;

namespace HomePageBackend.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<MenuCategory> MenuCategories { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Roles", "FoodOrderingSystem");
                entity.HasKey(e => e.RoleID);
                entity.Property(e => e.RoleName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Description).IsRequired().HasMaxLength(500);
            });
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users", "FoodOrderingSystem");
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Phone).HasMaxLength(15);
                entity.Property(e => e.PasswordHash).IsRequired().HasMaxLength(256);
                entity.Property(e => e.PasswordSalt).IsRequired().HasMaxLength(256);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasOne(e => e.RoleNavigation).WithMany(r => r.Users).HasForeignKey(e => e.Role);
            });
            
            modelBuilder.Entity<State>(entity =>
            {
                entity.ToTable("States", "FoodOrderingSystem");
                entity.HasKey(e => e.StateId);
                entity.Property(e => e.StateName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasIndex(e => e.StateName).IsUnique();
            });
            
            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("Cities", "FoodOrderingSystem");
                entity.HasKey(e => e.CityId);
                entity.Property(e => e.CityName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.IsActive).HasDefaultValue(true);
                entity.HasOne(e => e.State).WithMany(s => s.Cities).HasForeignKey(e => e.StateId);
            });
            
            modelBuilder.Entity<Restaurant>(entity =>
            {
                entity.ToTable("Restaurants", "FoodOrderingSystem");
                entity.HasKey(e => e.RestaurantId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(150);
                entity.Property(e => e.Description).HasMaxLength(300);
                entity.Property(e => e.Address).HasMaxLength(300);
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.User).WithMany(u => u.Restaurants).HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.City).WithMany(c => c.Restaurants).HasForeignKey(e => e.CityId);
                entity.HasOne(e => e.State).WithMany(s => s.Restaurants).HasForeignKey(e => e.StateId);
            });
            
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Addresses", "FoodOrderingSystem");
                entity.HasKey(e => e.AddressId);
                entity.Property(e => e.AddressLine).HasMaxLength(300);
                entity.Property(e => e.IsDefault).HasDefaultValue(false);
                
                entity.HasOne(e => e.User).WithMany(u => u.Addresses).HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.City).WithMany(c => c.Addresses).HasForeignKey(e => e.CityId);
                entity.HasOne(e => e.State).WithMany(s => s.Addresses).HasForeignKey(e => e.StateId);
            });
            
            modelBuilder.Entity<MenuCategory>(entity =>
            {
                entity.ToTable("MenuCategories", "FoodOrderingSystem");
                entity.HasKey(e => e.CategoryId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.HasOne(e => e.Restaurant).WithMany(r => r.MenuCategories).HasForeignKey(e => e.RestaurantId);
            });
            
            modelBuilder.Entity<MenuItem>(entity =>
            {
                entity.ToTable("MenuItems", "FoodOrderingSystem");
                entity.HasKey(e => e.MenuItemId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(300);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                entity.Property(e => e.IsAvailable).HasDefaultValue(true);
                entity.Property(e => e.ImageUrl).HasMaxLength(500);
                
                entity.HasOne(e => e.Category).WithMany(c => c.MenuItems).HasForeignKey(e => e.CategoryId);
            });
            
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts", "FoodOrderingSystem");
                entity.HasKey(e => e.CartId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                entity.HasOne(e => e.User).WithMany(u => u.Carts).HasForeignKey(e => e.UserId);
            });
            
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.ToTable("CartItems", "FoodOrderingSystem");
                entity.HasKey(e => e.CartItemId);
                entity.HasOne(e => e.Cart).WithMany(c => c.CartItems).HasForeignKey(e => e.CartId);
                entity.HasOne(e => e.MenuItem).WithMany(m => m.CartItems).HasForeignKey(e => e.MenuItemId);
            });
            
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Orders", "FoodOrderingSystem");
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.User).WithMany(u => u.Orders).HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Restaurant).WithMany(r => r.Orders).HasForeignKey(e => e.RestaurantId);
                entity.HasOne(e => e.Address).WithMany(a => a.Orders).HasForeignKey(e => e.AddressId);
            });
            
            modelBuilder.Entity<OrderItem>(entity =>
            {
                entity.ToTable("OrderItems", "FoodOrderingSystem");
                entity.HasKey(e => e.OrderItemId);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
                
                entity.HasOne(e => e.Order).WithMany(o => o.OrderItems).HasForeignKey(e => e.OrderId);
                entity.HasOne(e => e.MenuItem).WithMany(m => m.OrderItems).HasForeignKey(e => e.MenuItemId);
            });
            
            modelBuilder.Entity<Review>(entity =>
            {
                entity.ToTable("Reviews", "FoodOrderingSystem");
                entity.HasKey(e => e.ReviewId);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETDATE()");
                
                entity.HasOne(e => e.User).WithMany(u => u.Reviews).HasForeignKey(e => e.UserId);
                entity.HasOne(e => e.Restaurant).WithMany(r => r.Reviews).HasForeignKey(e => e.RestaurantId);
            });
        }
    }
}