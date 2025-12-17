using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomePageBackend.Models;

namespace HomePageBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuCategory>>> GetCategories()
        {
            return await _context.MenuCategories
                .Include(c => c.Restaurant)
                .ToListAsync();
        }

        [HttpGet("restaurant/{restaurantId}")]
        public async Task<ActionResult<IEnumerable<MenuCategory>>> GetCategoriesByRestaurant(int restaurantId)
        {
            return await _context.MenuCategories
                .Where(c => c.RestaurantId == restaurantId)
                .Include(c => c.MenuItems)
                .ToListAsync();
        }
    }
}