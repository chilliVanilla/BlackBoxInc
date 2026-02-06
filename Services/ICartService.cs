using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ICartService
    {
        public List<User> GetAllUserCarts();//
        List<CartItemResponseDto> GetCartItemsById(int id);//
        public bool  DeleteUser(int id);//
        public decimal CartTotal(int id);
        public int CreateNewUser(); //This should return the cart number
        public CartItem AddProductToUserCart(CartDto dto);
        public User RemoveProductFromUserCart(CartDto dto);
        
    }
}
