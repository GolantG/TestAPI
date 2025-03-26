using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestAPI.Models;
using TestAPI.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserMockService _userMockService;

        public AuthController(IUserMockService userMockService)
        {
            _userMockService = userMockService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] UserDTO user)
        {
            if (_userMockService.Register(user))
                return Ok(new { message = "User registered" });
            return BadRequest(new { message = "User already exists" });
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] UserDTO user)
        {
            var token = _userMockService.Authenticate(user);
            if (token == null)
                return Unauthorized(new { message = "Invalid user name or password" });
            return Ok(new { token });
        }

        [HttpGet("secure")]
        [Authorize]
        public IActionResult SecureEndpoint()
        {
            return Ok(new { message = $"Hi {_userMockService.GetCurrentUser()!.Name}! This is a secure endpoint." });
        }
    }
}
