using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public class ProductService : IProductServices
    {
        private readonly ApplicationDbContext dbContext;

        public ProductService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        //GetAllProducts
        public List<Products> GetAllProducts()
        {
            var allProducts = dbContext.Products.ToList();
            return allProducts;
        }

        //GetElementById
        public Products? GetById(int id)
        {
            return dbContext.Products.Find(id);
        }

        //AddProduct
        public Products? AddProduct(Products addProductsDto)
        {
            var product = dbContext.Products.ToList();
            var productEntity = new Products()
            {
                Name = addProductsDto.Name,
                Price = addProductsDto.Price,
                ProductDescription = addProductsDto.ProductDescription,
                InStock = addProductsDto.InStock,
                StockCount = addProductsDto.StockCount
            };

            var test = dbContext.Products.Any(p => p.Name == productEntity.Name);

            if (test)
            {
                return null;
            }

            dbContext.Products.Add(productEntity);
            dbContext.SaveChanges();
            return productEntity;
        }


    }
}
