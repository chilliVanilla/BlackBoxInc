using BlackBoxInc.Data;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;


namespace BlackBoxInc.Controllers
{

    /// <summary>
    /// Handles all product related operations including:
    /// adding new products, getting all products, sorting products based on category and name,
    /// deleting products and getting a product based on it's ID.
    /// </summary>
    /// <remarks>
    /// This controller exposes endpoints for managing products.
    /// All product operations are performed through this controller.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductServices productService;

        public ProductController(IProductServices productService)
        {
            this.productService = productService ?? throw new ArgumentNullException(nameof(productService)); //Study this exception type later
        }

        /// <summary>
        /// Gets all products stored in the database, whether in stock or not.
        /// </summary>
        /// <returns>
        /// Returns a list of all products and their details such as price, availability status, category, stock count and description.
        /// </returns>
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

        /// <summary>
        /// Adds a new product to the database and assigns a unique product ID.
        /// </summary>
        /// <param name="addProductsDto">
        /// Accepts the product details i.e name, price, category, description and availability status.
        /// </param>
        /// <returns>
        /// Returns a 201 status code and the product details if successfully created or a 409 status code if the product already exists.
        /// </returns>
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


        /// <summary>
        /// Returns a product's details when it's ID is passed in.
        /// </summary>
        /// <param name="id">
        /// The target product's ID.
        /// </param>
        /// <returns>
        /// Returns all the details of the specified product.
        /// </returns>
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

        /// <summary>
        /// Returns a  list of products that contain the name entered.
        /// </summary>
        /// <param name="name">
        /// The name of the target products.
        /// </param>
        /// <returns>
        /// Returns all details of the specified product.
        /// </returns>
        [HttpGet("FindByName")]
        public IActionResult GetProductByName(string name)
        {
            var matchingProducts = productService.GetByName(name);

            if (matchingProducts is null)
                return NotFound("No product with similar name");

            return Ok(matchingProducts);

        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="id">
        /// The ID of the product to be deleted.
        /// </param>
        /// <returns>
        /// Returns a status 204 code if successfully deleted 
        /// and a status 404 code when the item could not be found (Doesn't exist or was already deleted).
        /// </returns>
        [HttpDelete]
        [Route("{id:int}/deleteProduct")]
        public IActionResult DeleteProduct(int id)
        {
            var test = productService.RemoveProduct(id);
            if (test == null)
            {
                return NotFound("Item could not be deleted since it does not exist!!");
            }
            return NoContent();
        }

        /// <summary>
        /// Returns a list of products that belong to the category inputed.
        /// </summary>
        /// <param name="category">
        /// The category to be returned.
        /// </param>
        /// <returns>
        /// Returns a list of all products and their details based on the category inputed.
        /// </returns>
        [HttpGet ("products/{category}")]
        public IActionResult GetByCategory(string category)
        {
            var item = productService.GetByCategory(category);

            if (item is null)
            {
                return NotFound("No products exist in that category!");
            }

            return Ok(item);
        }


    }
}
