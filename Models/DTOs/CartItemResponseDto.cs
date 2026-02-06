namespace BlackBoxInc.Models.DTOs
{
    public class CartItemResponseDto
    {
        //public int UserId { get; set; }
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public required string Name { get; set; }
        public required decimal Price { get; set; }
        public int Quantity { get; set; }

    }
}
