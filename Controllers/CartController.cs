using HomePageBackend.DTOs;
using HomePageBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomePageBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddToCart([FromQuery] int userId, [FromBody] AddToCartDto dto)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            if (dto.MenuItemId <= 0 || dto.Quantity <= 0)
                return BadRequest(new { message = "Invalid menu item or quantity" });

            var cart = await _cartService.AddToCartAsync(userId, dto);
            return Ok(new { success = true, data = cart });
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            var cart = await _cartService.GetCartAsync(userId);
            return Ok(new { success = true, data = cart });
        }

        [HttpDelete("item/{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart([FromQuery] int userId, int cartItemId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            var result = await _cartService.RemoveFromCartAsync(userId, cartItemId);
            if (!result)
                return NotFound(new { message = "Cart item not found" });

            return Ok(new { success = true, message = "Item removed from cart" });
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart([FromQuery] int userId)
        {
            if (userId <= 0)
                return BadRequest(new { message = "Invalid user ID" });

            var result = await _cartService.ClearCartAsync(userId);
            if (!result)
                return NotFound(new { message = "Cart not found" });

            return Ok(new { success = true, message = "Cart cleared" });
        }
    }
}
