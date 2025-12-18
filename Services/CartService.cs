using HomePageBackend.DTOs;
using HomePageBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace HomePageBackend.Services
{
    public interface ICartService
    {
        Task<CartDto> AddToCartAsync(int userId, AddToCartDto dto);
        Task<CartDto> GetCartAsync(int userId);
        Task<bool> RemoveFromCartAsync(int userId, int cartItemId);
        Task<bool> ClearCartAsync(int userId);
    }

    public class CartService : ICartService
    {
        private readonly AppDbContext _context;

        public CartService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CartDto> AddToCartAsync(int userId, AddToCartDto dto)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.MenuItemId == dto.MenuItemId);

            if (existingItem != null)
            {
                existingItem.Quantity += dto.Quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    MenuItemId = dto.MenuItemId,
                    Quantity = dto.Quantity
                };
                _context.CartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();

            return await MapToCartDtoAsync(cart.CartId);
        }

        public async Task<CartDto> GetCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return new CartDto { UserId = userId };
            }

            return await MapToCartDtoAsync(cart.CartId);
        }

        public async Task<bool> RemoveFromCartAsync(int userId, int cartItemId)
        {
            var cartItem = await _context.CartItems
                .Include(ci => ci.Cart)
                .FirstOrDefaultAsync(ci => ci.CartItemId == cartItemId && ci.Cart.UserId == userId);

            if (cartItem == null)
                return false;

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return false;

            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<CartDto> MapToCartDtoAsync(int cartId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.MenuItem)
                .FirstOrDefaultAsync(c => c.CartId == cartId);

            if (cart == null)
                return new CartDto();

            var cartItems = cart.CartItems.Select(ci => new CartItemDto
            {
                CartItemId = ci.CartItemId,
                MenuItemId = ci.MenuItemId,
                MenuItemName = ci.MenuItem.Name,
                Quantity = ci.Quantity,
                Price = ci.MenuItem.Price,
                Total = ci.Quantity * ci.MenuItem.Price
            }).ToList();

            return new CartDto
            {
                CartId = cart.CartId,
                UserId = cart.UserId,
                CartItems = cartItems,
                Total = cartItems.Sum(ci => ci.Total)
            };
        }
    }
}
