using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ICartService
    {
        List<CartItemResponseDto> GetCartItemsById(string id);//
        public decimal CartTotal(string id);
        public CartItem AddProductToUserCart(CartDto dto);
        public User RemoveProductFromUserCart(CartDto dto);
        
    }
}
