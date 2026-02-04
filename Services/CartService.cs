using BlackBoxInc.Data;
using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlackBoxInc.Services
{
    public class CartService : ICartService
    {
        private readonly ApplicationDbContext dbContext;
        public CartService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<Cart> GetAllCarts()
        {
            var allCarts = dbContext.Carts.ToList();
            return allCarts;
        }

        public List<CartItemResponseDto> GetCartItemsById(int id)
        {
            var cart = dbContext.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(r => r.CartId == id);
            if (cart is null)
            {
                return null;
            }

            var allItems = cart.Items.Select(i => new CartItemResponseDto
            {
                ProductId = i.ProductId,
                Name = i.Name,
                Price = i.Price
            }).ToList();

            return allItems;

        }

        public int CreateNewCart()
        {
            var newCart = new Cart();

            dbContext.Carts.Add(newCart);
            dbContext.SaveChanges();

            return newCart.CartId;
        }

        public bool DeleteCart(int id)
        {
            var cart = dbContext.Carts.Find(id);
            if (cart is null)
            {
                return false;
            }

            dbContext.Carts.Remove(cart);
            dbContext.SaveChanges();
            return true;
        }

        public decimal CartTotal(int id)
        {
            var prodIds = dbContext.Carts
                .Where(c => c.CartId == id)
                .SelectMany(c => c.Items)
                .Select(c => c.ProductId).ToList();

            var prices = dbContext
                .Products
                .Where(p => prodIds.Contains(p.ProductId))
                .Select(p => p.Price)
                .ToList();
            decimal total = prices.Sum();
            return total;

        }

        public Cart AddProductToCart(CartDto dto)
        {
            var item = dbContext.Products.Find(dto.ProductId);

            var selectCart = dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefault(ca => ca.CartId == dto.CartId);
            var exists = selectCart.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            if (exists != null)
            { 
                return null;
            }

            if (selectCart is null)
                return null;

            if (item is null)
                return null;

            Console.WriteLine("In the Create new cart method in the cart service class");//

            var product = new CartItem()
            {
                ProductId = dto.ProductId,
                Product = item,

                CartId = selectCart.CartId,

                Name = item.Name,
                Price = item.Price,
                ProductDescription = item.ProductDescription
            };
            selectCart.Items.Add(product);
            dbContext.SaveChanges();
            return selectCart;

        }


        public Cart RemoveProductFromCart(CartDto dto)
        {

            var selectCart = dbContext.Carts
                .Include(c => c.Items)
                .FirstOrDefault(ca => ca.CartId == dto.CartId);

            var item = selectCart
                .Items
                .FirstOrDefault(c => c.ProductId == dto.ProductId);

            if (selectCart is null)
                return null;

            if (item is null)
                return null;

            selectCart.Items.Remove(item);
            dbContext.CartItems.Remove(item);
            dbContext.SaveChanges();
            return selectCart;

        }
    }


}
