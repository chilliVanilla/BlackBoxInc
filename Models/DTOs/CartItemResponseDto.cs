namespace BlackBoxInc.Models.DTOs
{
    public class CartItemResponseDto
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public decimal Price { get; set; }
    }
}