using BlackBoxInc.Data;
using BlackBoxInc.Models.DTOs;
using Microsoft.EntityFrameworkCore;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;


namespace BlackBoxInc.UnitTest;

public class CartServiceTest
{
    [Fact]
    public void CartTotal_CalculatesTotal_ForSpecifiedUser()
    {
        //Arrange
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        
        //Assert
        using (var context = new ApplicationDbContext(options))
        {
            var product = new Products { ProductId = 1, Price = 50, Category = "NULL", Name = "NULL"};
            context.Products.Add(product);
            context.Add(new CartItem { UserId = "jesse_000", ProductId = 1, Quantity = 3 });
            context.SaveChanges();
        }
        //Act
        using (var context = new ApplicationDbContext(options))
        {
            var service = new CartService(context);
            var total = service.CartTotal("jesse_000");
            Assert.Equal(150, total);
        }
    }

    [Fact]
    public void GetCartItems_ByID()
    {
        //Arrange
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        //Assert
        using (var context = new ApplicationDbContext(options)) 
        {
            var product1 = new Products { ProductId = 1, Price = 50, Category = "NULL", Name = "prod_1" };
            var product2 = new Products { ProductId = 2, Price = 65, Category = "NULL", Name = "prod_2"};
            context.Products.Add(product1);
            context.Products.Add(product2);
            context.Users.Add(new User { Id = "dave001"});
            context.CartItems.Add(new CartItem { UserId = "dave001", ProductId = 1, Quantity = 2});
            context.CartItems.Add(new CartItem { UserId = "dave001", ProductId = 2, Quantity = 6});
            context.SaveChanges();
        }
        //Act
        using (var context = new ApplicationDbContext(options))
        {
            var service = new CartService(context);
            var userCart = service.GetCartItemsById("dave001");
            
            // if (userCart is null)
            // {
            //     Console.WriteLine("User cart is null!!!");
            // }
            // else
            // {
            //     Console.WriteLine("User cart is not null!!!");
            // }
            var prod1 = userCart[0].Name;
            Assert.NotNull(userCart);
            Assert.Equal("prod_1", prod1);
        }
    }

    [Fact]
    public void AddProduct_ToSpecificUserCart()
    {
        //Arrange
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        
        //Assert
        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Add(new User { Id = "jesse001"});
            context.Products.Add(new Products { ProductId = 1, Category = "NULL", Name = "product_1", Price = 23, StockCount = 140});
            // context.CartItems.Add(new CartItem { UserId = "jesse001", ProductId = 1, Quantity = 10});
            context.SaveChanges();
        }
        
        //Act
        using (var context = new ApplicationDbContext(options))
        {
            var service = new CartService(context);
            // var input = new CartItemResponseDto()
            var test = service.AddProductToUserCart(new CartDto
            {
                ProductId = 1,
                UserId = "jesse001",
                Quantity = 4
            });
            
            // if (test is null)
            // {
            //     Console.WriteLine("check cart is null!!!");
            // }
            // else
            // {
            //     Console.WriteLine("check cart is not null!!!");
            // }
            var check = service.GetCartItemsById("jesse001");
            Assert.Equal("product_1", check[0].Name);
        }
    }

    [Fact]
    public void RemoveProduct_FromSelectCart()
    {
        //Arrange
        var dbName = Guid.NewGuid().ToString();
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;
        
        //Assert
        using (var context = new ApplicationDbContext(options))
        {
            context.Users.Add(new User { Id = "user001"});
            context.Products.Add(new Products { ProductId = 1, Name = "product1", Category = "NULL", Price = 123000});
            context.CartItems.Add(new CartItem { UserId = "user001", ProductId = 1, Quantity = 3});
            context.SaveChanges();
        }
        
        //Act
        using (var context = new ApplicationDbContext(options))
        {
            var service = new CartService(context);
            service.RemoveProductFromUserCart(new CartDto
            {
                UserId = "user001",
                ProductId = 1,
                Quantity = 2
            });
            var user = service.GetCartItemsById("user001");
            Assert.Equal(1, user[0].Quantity);
        }
    }
    
    // [Fact]
    
}