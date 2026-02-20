//using BlackBoxInc.Migrations;
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
                RefreshToken =  _tokenService.GenerateRefreshToken(),
                RefreshTokenExpiry = DateTime.Now.AddDays(1)
            };

            var result = await _userManager.CreateAsync(user, signUpDto.password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            //assign user role
            await _userManager.AddToRoleAsync(user, "User");

            return Ok(new
            {
                Username = user.UserName,
                AccessToken = _tokenService.GenerateToken(user),
                Role = user,
                RefreshToken = _tokenService.GenerateRefreshToken(),
            });

        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid Credentials");
            var token = _tokenService.GenerateToken(user);
            return Ok(new
            {
                AccessToken = token,
                RefreshToken = user.RefreshToken
            });
        }
        
        [HttpPost("refresh")]
        public async Task<ActionResult<UserDto>> Refresh(TokenRequestDto tokenRequest)
        {
            var principal = _tokenService.GetPrincipalFromJwtAccessToken(tokenRequest.AccessToken);
            Console.WriteLine("Principal: " + principal);
            
            var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine("ID: " + id);

            var user = await _userManager.FindByIdAsync(id);
            //Tests that should be logged, not printed to console, for better workflow
            if (user == null)
            {
                Console.WriteLine("User is null");
            }else if (user.RefreshToken != tokenRequest.RefreshToken)
            {
                Console.WriteLine("Invalid refresh token!!!");
                Console.WriteLine("User.RefreshToken: " + user.RefreshToken);
                Console.WriteLine("Token.RefreshToken: " + tokenRequest.RefreshToken);
            }else if (user.RefreshTokenExpiry < DateTime.Now)
            {
                Console.WriteLine("Refresh token expired");
            }

            if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiry <= DateTime.Now)
            {
                return Unauthorized("Invalid refresh token attempt");
            }

            

            var newAccessToken = await _tokenService.GenerateToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            // Since renewing both is more secure against attackers

            user.RefreshToken = newRefreshToken;
            
            await _userManager.UpdateAsync(user);

            return new UserDto
            {
                Username = user.UserName,
                AccessToken = newAccessToken, 
                RefreshToken = newRefreshToken,
                RefreshTokenExpiry = user.RefreshTokenExpiry
            };
        }


        [Authorize]
        [HttpGet]
        public IActionResult GetSecret()
        {
            return Ok("It works, right?");
        }
    }
}
