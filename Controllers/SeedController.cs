using Microsoft.AspNetCore.Mvc;
using HomePageBackend.Models;

namespace HomePageBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeedController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SeedController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            var restaurantCount = _context.Restaurants.Count();
            var menuItemCount = _context.MenuItems.Count();
            return Ok(new { RestaurantCount = restaurantCount, MenuItemCount = menuItemCount });
        }

        [HttpPost]
        public async Task<IActionResult> SeedData()
        {
            return Ok("Seeding disabled - use existing database data");
        }
    }
}