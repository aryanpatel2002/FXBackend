namespace HomePageBackend.DTOs
{
    public class AddToCartDto
    {
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
