using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface IProductServices
    {
        List<Products> GetAllProducts();
        Products? GetById(int id);
        Products? AddProduct(Products product);
        public List<Products> GetByName(string name);
        public Products RemoveProduct(int id);
        public List<Products> GetByCategory(string category);
    }
}
