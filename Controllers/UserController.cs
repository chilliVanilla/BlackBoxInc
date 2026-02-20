using BlackBoxInc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BlackBoxInc.Controllers
{


    [ApiController]
    [Route("api/[controller]")]
    public class UserController:ControllerBase

    {

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }


        // /// <summary>
        // /// Creates new users
        // /// </summary>
        // /// <remarks>
        // /// This endpoint initializes an empty cart and returns the generated cart ID
        // /// </remarks>
        // /// <returns>
        // /// Returns the unique identifier of the newly created cart(user)
        // /// </returns>
        // Post: Users
        // [Authorize]
        // [HttpPost("addNewUser")]
        // public IActionResult AddNewUser()
        // {
        //     string userId = userService.CreateNewUser();
        //
        //     return Ok(userId);
        // }


        /// <summary>
        /// Returns all users created
        /// </summary>
        /// <returns>
        /// A list of all created carts
        /// </returns>
        //GET: All carts
        [Authorize(Roles = "Admin")]
        [HttpGet("AllUsers")]
        //[Route("AllCarts")]
        public IActionResult GetAllUsers()
        {

            return Ok(userService.GetAllUsers());
        }


        /// <summary>
        /// Deletes a user based on the user ID
        /// </summary>
        /// <param name="DelId">
        /// ID of the user to be deleted
        /// </param>
        /// <returns>
        /// Returns status code 204 if successfully deleted and 404 if the user was not found
        /// </returns>
        //DELETE:By Id
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{userId:int}")]
        public IActionResult DeleteCart(int userId)
        {
            var test = userService.DeleteUser(userId);
            if (test == false)
            {
                return NotFound("User not found");
            }
            return NoContent();
        }
    }
}
