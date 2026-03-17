using BlackBoxInc.Models.DTOs;
using BlackBoxInc.Models.Entities;
using BlackBoxInc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Hangfire;


namespace BlackBoxInc.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;
        private readonly IEmailService _mailBoy;

        public AuthController(ITokenService tokenService, UserManager<User> userManager, ILogger<AuthController> logger, IEmailService mailBoy)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _logger = logger;
            _mailBoy = mailBoy;
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
                RefreshTokenExpiry = DateTime.Now.AddDays(3)
            };

            var result = await _userManager.CreateAsync(user, signUpDto.password);

            if (!result.Succeeded) return BadRequest(result.Errors);
            //assign user role
            await _userManager.AddToRoleAsync(user, "Admin");
            await _mailBoy.SendEmailAsync(user.Email, "Welcome To BlackBoxInc",
                "Hello to BlackBoxInc (Formerly know as BrainBoxInc).\nWe hope to have you as a valued customer years from now, thank you.");
            

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
            if (user != null)
            {
                _logger.LogInformation("User Found: " + user.UserName + "!!");
            }
            else
            {
                _logger.LogError("User - " + loginDto.Username + " not found during login at " + DateTime.UtcNow.Date);
            }
            var confirmationMail = "Notice!!\nA sign in with your account was made into BlackBoxInc, was this you??";

            if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
                return Unauthorized("Invalid Credentials");
            var token = _tokenService.GenerateToken(user);
            //Send notification mail
            await _mailBoy.SendEmailAsync(user.Email, "Confirmation Email", confirmationMail);
            RecurringJob.AddOrUpdate<IEmailService>("Follow Up", x => x.SendEmailAsync("ikeoluwa.jesse@gmail.com", "Reminder", "This is a friendly reminder that you are currently logged in on the application.  If you have already signed out and you recieve this mail, kindly contact support"), "*/2 * * * * *");
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
                _logger.LogError("User is null");
            }else if (user.RefreshToken != tokenRequest.RefreshToken)
            {
                _logger.LogError("Invalid refresh token!!!");
                _logger.LogInformation("User.RefreshToken: " + user.RefreshToken);
                _logger.LogInformation("Token.RefreshToken: " + tokenRequest.RefreshToken);
            }else if (user.RefreshTokenExpiry < DateTime.Now)
            {
                _logger.LogError("Refresh token expired");
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
        [HttpGet ("Test Endpoint")]
        public IActionResult GetSecret()
        {
            return Ok("It works, right?");
        }
        
        [HttpGet("Test(Unauthorized)")]
        public IActionResult UnAuth()
        {
            _logger.LogInformation("In the test endpoint");
            _logger.LogDebug("A debug to be logged!!!!!!");
            _logger.LogCritical("A sample critical warning --------------------------");
            return Ok("Test endpoint for logs");
        }
    }
}
