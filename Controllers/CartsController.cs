using Microsoft.AspNetCore.Mvc;
using BlackBoxInc.Services;
using BlackBoxInc.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

/// <summary>
/// Handles all shopping cart related operations such as creating carts(users),
/// adding products to cart, retrieving carts for both individual users and all user contents, 
/// clearing carts, and getting the total price of items in a cart.
/// </summary>
/// <remarks>
/// This controller exposes endpoints for managing users and their shopping carts.
/// All cart operations are performed through this controller.
/// </remarks>

namespace BlackBoxInc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartsController : ControllerBase
    {
        //private readonly ApplicationDbContext dbContext;
        private readonly ICartService cartService;

        public CartsController(ICartService cartService)
        {
            this.cartService = cartService;
        }


        /// <summary>
        /// Adds a product to an existing cart and if the product already exists in the cart, it increments it's quantity
        /// </summary>
        /// <param name="dto">
        /// The dto passed contains the product ID, quantity and the target cart number
        /// </param>
        /// <returns>
        /// Returns the product details of the product added
        /// </returns>
        [Authorize]
        [HttpPost("addNewProductToCart")]
        public IActionResult AddItemToCart([FromBody] CartDto dto)
        {
            var cart = cartService.AddProductToUserCart(dto);
            if (cart is null)
                return NotFound("Cart or Product not found\nOr product already exists!!!!");

            //return Ok(cart);
            return Ok(new CartItemResponseDto
            {
                Name = cart.Product.Name,
                Price = cart.Product.Price,
                Quantity = cart.Quantity
            });
        }


        /// <summary>
        /// Returns the user's cart based on the inputed user ID
        /// </summary>
        /// <param name="userId">
        /// ID of the user whose cart is to be modified
        /// </param>
        /// <returns>
        /// All item  and cart details of the user
        /// </returns>
        //GET:By Id
        [Authorize]
        [HttpGet]
        [Route("{userId:int}/cartById")]
        public IActionResult GetCartById(string userId)
        {
            var cartProducts = cartService.
                GetCartItemsById(userId);
            if (cartProducts is null)
            {
                return NotFound();
            }
            return Ok(cartProducts);

        }



        /// <summary>
        /// Returns the total amount of items in a user's cart
        /// </summary>
        /// <param name="id">
        /// The ID of the user whose cart is to be modified
        /// </param>
        /// <returns>
        /// Returns a total based on the prices and quantities of each item in the cart.
        /// </returns>
        [Authorize]
        [HttpGet]
        [Route("{id:int}/total")]
        public IActionResult GetTotalInCart(string id)
        {
            var total = cartService.CartTotal(id);
            return Ok(total);
        }


        /// <summary>
        /// Deletes an item from a cart
        /// </summary>
        /// <param name="dto">
        /// Accepts the user ID, product ID and number of products to be deleted or removed
        /// </param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("DeleteCartItem")]
        public IActionResult DeleteItemCart([FromBody]CartDto dto)
        {
            if(cartService.RemoveProductFromUserCart(dto) is null)
                return NotFound("Product or cart not found");

            return Ok("Product deleted");
        }

        
    }

}
