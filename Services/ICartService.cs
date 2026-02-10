using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ICartService
    {
        List<CartItemResponseDto> GetCartItemsById(int id);//
        public decimal CartTotal(int id);
        public CartItem AddProductToUserCart(CartDto dto);
        public User RemoveProductFromUserCart(CartDto dto);
        
    }
}
