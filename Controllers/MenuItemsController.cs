using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HomePageBackend.Models;

namespace HomePageBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuItemsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(AppDbContext context, ILogger<MenuItemsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            try
            {
                var menuItems = await _context.MenuItems
                    .Where(m => m.IsAvailable)
                    .Include(m => m.Category)
                    .ThenInclude(c => c.Restaurant)
                    .ToListAsync();
                
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu items");
                return StatusCode(500, "An error occurred while retrieving menu items");
            }
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItemsByCategory(int categoryId)
        {
            try
            {
                if (categoryId <= 0)
                    return BadRequest("Invalid category ID");

                var menuItems = await _context.MenuItems
                    .Where(m => m.IsAvailable && m.CategoryId == categoryId)
                    .Include(m => m.Category)
                    .ThenInclude(c => c.Restaurant)
                    .ToListAsync();
                
                return Ok(menuItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu items for category {CategoryId}", categoryId);
                return StatusCode(500, "An error occurred while retrieving menu items");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest("Invalid menu item ID");

                var menuItem = await _context.MenuItems
                    .Include(m => m.Category)
                    .ThenInclude(c => c.Restaurant)
                    .FirstOrDefaultAsync(m => m.MenuItemId == id);

                if (menuItem == null)
                    return NotFound($"Menu item with ID {id} not found");

                return Ok(menuItem);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item {MenuItemId}", id);
                return StatusCode(500, "An error occurred while retrieving the menu item");
            }
        }
    }
}