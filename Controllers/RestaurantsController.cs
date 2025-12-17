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
        private readonly ILogger<RestaurantsController> _logger;

        public RestaurantsController(AppDbContext context, ILogger<RestaurantsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Restaurant>>> GetRestaurants()
        {
            try
            {
                var restaurants = await _context.Restaurants
                    .Where(r => r.Status == "Approved")
                    .Include(r => r.MenuCategories)
                    .ThenInclude(c => c.MenuItems)
                    .ToListAsync();
                
                return Ok(restaurants);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurants");
                return StatusCode(500, "An error occurred while retrieving restaurants");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Restaurant>> GetRestaurant(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid restaurant ID");

                var restaurant = await _context.Restaurants
                    .Include(r => r.MenuCategories)
                    .ThenInclude(c => c.MenuItems)
                    .FirstOrDefaultAsync(r => r.RestaurantId == id);

                if (restaurant == null)
                    return NotFound($"Restaurant with ID {id} not found");

                return Ok(restaurant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurant {RestaurantId}", id);
                return StatusCode(500, "An error occurred while retrieving the restaurant");
            }
        }
    }
}