using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface ICartService
    {
        public List<Cart> GetAllCarts();//
        List<CartItemResponseDto> GetCartItemsById(int id);//
        public bool  DeleteCart(int id);//
        public decimal CartTotal(int id);
        public int CreateNewCart(); //This should return the cart number
        public Cart AddProductToCart(CartDto dto);
        public Cart RemoveProductFromCart(CartDto dto);
        
    }
}
