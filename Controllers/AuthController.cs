using BlackBoxInc.Migrations;
using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlackBoxInc.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;

        public AuthController(ITokenService tokenService, UserManager<User> userManager)
        {
            _tokenService = tokenService;
            _userManager = userManager;
        }


        [HttpPost("Sign Up")]
        public async Task<IActionResult> Register([FromBody] SignUpDto signUpDto)
        {
            if (await _userManager.Users.AnyAsync(x => x.UserName == signUpDto.Username))
            {
                return BadRequest("Username is taken!!");
            }

            var user = new User
            {
                FirstName = signUpDto.FirstName,
                LastName = signUpDto.LastName,
                Email = signUpDto.Email,
                UserName = signUpDto.Username,
            };

            var result = await _userManager.CreateAsync(user, signUpDto.password);

            if (result.Succeeded) 
            {
                //assign user role
                await _userManager.AddToRoleAsync(user, "User");

                return Ok(new
                {
                    Username = user.UserName,
                    Token = _tokenService.GenerateToken(user),
                    Role = user
                });
            }
            
            return BadRequest(result.Errors);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);
            
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(new { Token = token });
            }

            return Unauthorized("Invalid Credentials");
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok("It works, right?");
        }
    }
}
