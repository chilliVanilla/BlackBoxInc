namespace BlackBoxInc.Models.DTOs
{
    internal class CartResponseDto
    {
        public int CartId { get; set; }
        public List<CartItemResponseDto> Items { get; set; }
    }
}