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

        //public void RemoveDuplicates()
        //{
        //    var cartItems = dbContext.CartItems.ToList();
        //}

        public List<User> GetAllUserCarts()
        {
            var allCarts = dbContext.Users
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .ToList();
            return allCarts;
        }

        public List<CartItemResponseDto> GetCartItemsById(int id)
        {
            var cart = dbContext.Users
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefault(r => r.UserId == id);

            if (cart is null)
            {
                return null;
            }

            var allItems = cart.Items.Select(i => new CartItemResponseDto
            {
                ProductId = i.ProductId,
                Name = dbContext.Products.Find(i.ProductId).Name,
                Price = dbContext.Products.Find(i.ProductId).Price,
                Quantity = i.Quantity
            }).ToList();

            return allItems;

        }

        public int CreateNewUser()
        {
            var newUser = new User();

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();

            return newUser.UserId;
        }

        public bool DeleteUser(int id)
        {
            var user = dbContext.Users.Find(id);
            if (user is null)
            {
                return false;
            }

            dbContext.Users.Remove(user);
            dbContext.SaveChanges();
            return true;
        }

        public decimal CartTotal(int id)
        {
            var total = dbContext.CartItems
                .Where(ci => ci.UserId == id)
                .Select(c => c.Quantity * c.Product.Price)
                .Sum();

            return total;

        }

        public CartItem AddProductToUserCart(CartDto dto)
        {
            var item = dbContext.Products.Find(dto.ProductId);
            //get the item details
            var stock = item.StockCount;

            var selectUser = dbContext.Users
                .Include(c => c.Items)
                .FirstOrDefault(ca => ca.UserId == dto.UserId);
            //Get user details id, items list

            if (selectUser is null)
                return null;

            if (item is null)
                return null;

            var exists = selectUser.Items.FirstOrDefault(i => i.ProductId == dto.ProductId);
            //test to see if it already exists in cart

            if (exists != null)
            {
                exists.Quantity += dto.Quantity;

            }
            stock = stock - dto.Quantity;
            var convert = new CartItem
            {
                //UserId = dto.UserId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };

            selectUser.Items.Add(convert);
            //dbContext.Users.Add(selectUser);
            dbContext.SaveChanges();

            return convert;
            //return selectUser;///////////////////////////////////////////

        }

        public User RemoveProductFromUserCart(CartDto dto)
        {

            var selectCart = dbContext.Users
                .Include(c => c.Items)
                .FirstOrDefault(ca => ca.UserId == dto.UserId);

            var item = selectCart
                .Items
                .FirstOrDefault(c => c.ProductId == dto.ProductId);

            if (selectCart is null)
                return null;

            if (item is null)
                return null;
            if ((item.Quantity - dto.Quantity) > 0)
            {
                item.Quantity = item.Quantity - dto.Quantity;
            }
            else
            {
                selectCart.Items.Remove(item);
                dbContext.CartItems.Remove(item);
            }

            var stock = dbContext.Products.Find(dto.ProductId).StockCount;
            stock = stock + dto.Quantity;

            dbContext.SaveChanges();
            return selectCart;
        }
    }


}
