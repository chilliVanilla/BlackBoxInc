using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using System.Text.RegularExpressions;

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
            var productEntity = new Products()
            {
                Name = addProductsDto.Name,
                Price = addProductsDto.Price,
                ProductDescription = addProductsDto.ProductDescription,
                InStock = addProductsDto.InStock,
                StockCount = addProductsDto.StockCount,
                Category = addProductsDto.Category
            };

            //var compare = Regex.Replace(productEntity.Name.Trim(), @"\s+", "");
            //var prod = Regex.Replace(dbContext.Products.);

            var compare = Normalize(addProductsDto.Name);

            var test = dbContext.Products
                .AsEnumerable()
                .Any(p => Normalize(p.Name) == compare);


            //var results = dbContext.Products.AsEnumerable()
            //    .Where(p=>Matches(p.Name, addProductsDto.Name));
                

            if (test)
            {
                return null;
            }

            dbContext.Products.Add(productEntity);
            dbContext.SaveChanges();
            return productEntity;
        }


        public List<Products> GetByName(string name)
        {
            name = name.Trim().ToLower();
            var matchingProducts = dbContext.Products.Where(p => p.Name.Trim().ToLower().Contains(name)).ToList();
            if (matchingProducts is null)
                return null;

            return matchingProducts;
        }

        public Products RemoveProduct(int id)
        {
            var product = dbContext.Products.Find(id);
            
            if (product is null)
            {
                return null;
            }
            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return product;
        }


        public List<Products> GetByCategory(string category)
        {
            var prodList = dbContext.Products.Where(p=>p.Category.Contains(category.Trim().ToLower())).ToList();

            return prodList;
        }

        public bool Matches(string productName, string input)
        {
            var name = productName.ToLower().Trim();
            var tokens = input.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries);

            return tokens.All(token => name.Contains(token));
        }

        public string Normalize(string input)
        {
            var end = Regex.Replace(input.Trim().ToLower(), @"\s+", "");
            return end;
        }

    }
}
