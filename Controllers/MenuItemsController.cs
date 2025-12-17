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

        public MenuItemsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItems()
        {
            return await _context.MenuItems
                .Where(m => m.IsAvailable)
                .Include(m => m.Category)
                .ThenInclude(c => c.Restaurant)
                .ToListAsync();
        }

        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<MenuItem>>> GetMenuItemsByCategory(int categoryId)
        {
            return await _context.MenuItems
                .Where(m => m.IsAvailable && m.CategoryId == categoryId)
                .Include(m => m.Category)
                .ThenInclude(c => c.Restaurant)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MenuItem>> GetMenuItem(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.Category)
                .ThenInclude(c => c.Restaurant)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);

            if (menuItem == null)
            {
                return NotFound();
            }

            return menuItem;
        }
    }
}