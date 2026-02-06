namespace BlackBoxInc.Models.DTOs
{
    internal class UserResponseDto
    {
        public int UserId { get; set; }
        public required List<CartItemResponseDto> Items { get; set; }
    }
}