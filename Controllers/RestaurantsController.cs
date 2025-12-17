using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomePageBackend.Models;

namespace HomePageBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RestaurantsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            return await _context.Restaurants
                .Where(r => r.Status == "Approved")
                .Include(r => r.MenuCategories)
                .ThenInclude(c => c.MenuItems)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            var restaurant = await _context.Restaurants
                .Include(r => r.MenuCategories)
                .ThenInclude(c => c.MenuItems)
                .FirstOrDefaultAsync(r => r.RestaurantId == id);

            if (restaurant == null)
            {
                return NotFound();
            }

            return restaurant;
        }
    }
}