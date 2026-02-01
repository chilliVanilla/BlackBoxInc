using BlackBoxInc.Models.Entities;

namespace BlackBoxInc.Services
{
    public interface IProductServices
    {
        List<Products> GetAllProducts();
        Products? GetById(int id);
        Products? AddProduct(Products product);
    }
}
