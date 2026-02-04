using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;

namespace BlackBoxInc.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productService;

        public ProductController(IProductServices productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService)); //Study this exception type later
        }

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = productService.GetAllProducts();
            if (products == null)
            {
                return NotFound();
            }

            return Ok(products);
        }

        [HttpPost]
        public IActionResult AddNewProduct(Products addProductsDto)
        {
            var outcome = productService.AddProduct(addProductsDto);

            if (outcome is null)
            {
                return Conflict(new { message = $"Product with name {addProductsDto.Name} already exists!!" });//The proper RESTful alternative to an error 405
            }

            return CreatedAtAction(nameof(GetElementById), new { id = outcome.ProductId }, outcome);
        }


        [HttpGet]
        [Route("{id:int}")]
        public IActionResult GetElementById(int id)
        {
            var test = productService.GetById(id);

            if (test is null)
            {
                return NotFound();//Look for the proper RESTful return statement 
            }
            return Ok(test);
        }

        [HttpGet("FindByName")]
        public IActionResult GetProductByName(string name)
        {
            var matchingProducts = productService.GetByName(name);

            if (matchingProducts is null)
                return NotFound("No product with similar name");

            return Ok(matchingProducts);

        }

        [HttpDelete]
        [Route("{id:int}/deleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var test = productService.RemoveProduct(id);
            if (test == null)
            {
                return NotFound("Item could not be deleted since it does not exist!!");
            }
            return Ok("Delete Successful");
        }
    }
}
