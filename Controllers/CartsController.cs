using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using BlackBoxInc.Models.DTOs;

namespace BlackBoxInc.Controllers
{
    public class CartsController : Controller
    {
        //private readonly ApplicationDbContext dbContext;
        private readonly ICartService cartService;

        public CartsController(ICartService cartService)
        {
            this.cartService = cartService;
        }

        // Post: Carts
        [HttpPost("addNewCart")]
        //[Route("{dto:CartDto}")]
        public IActionResult AddNewCart()
        {
            int cartId = cartService.CreateNewCart();

            return Ok(cartId);
        }

        [HttpPost("addNewProductToCart")]
        public IActionResult AddItemToCart([FromBody] CartDto dto)
        {
            var cart = cartService.AddProductToCart(dto);
            if (cart is null)
                return NotFound("Cart or Product not found\nOr product already exists!!!!");

            //return Ok(cart);
            return Ok(new CartResponseDto
            {
                CartId = cart.CartId,
                Items = cart.Items.Select(i => new CartItemResponseDto
                {
                    CartItemId = i.CartItemId,
                    ProductId = i.ProductId,
                    Name = i.Name,
                    Price = i.Price
                }).ToList()
            });

        }


        //GET: All carts
        [HttpGet("AllCarts")]
        //[Route("AllCarts")]
        public IActionResult GetAllCarts()
        {
            return Ok(cartService.GetAllCarts());
        }

        //GET:By Id
        [HttpGet]
        [Route("{cartId:int}/cartById")]
        public IActionResult GetCartById(int cartId)
        {
            var cartProducts = cartService.
                GetCartItemsById(cartId);
            if (cartProducts is null)
            {
                return NotFound();
            }
            return Ok(cartProducts);

        }

        //DELETE:By Id
        [HttpDelete]
        [Route("{DelId:int}")]
        public IActionResult DeleteCart(int DelId)
        {
            var test = cartService.DeleteCart(DelId);
            if (test == false)
            {
                return NotFound("Cart not found");
            }
            return Ok();
        }

        [HttpGet]
        [Route("{id:int}/total")]
        public IActionResult GetTotalInCart(int id)
        {
            var total = cartService.CartTotal(id);
            return Ok(total);
        }

        [HttpDelete("DeleteCartItem")]
        public IActionResult DeleteItemCart([FromBody]CartDto dto)
        {
            if(cartService.RemoveProductFromCart(dto) is null)
                return NotFound("Product or cart not found");

            return Ok("Product deleted");
        }

        
    }

}
