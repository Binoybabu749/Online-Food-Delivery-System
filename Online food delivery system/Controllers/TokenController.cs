using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Online_food_delivery_system.Interface;
using Online_food_delivery_system.Models;
using Online_food_delivery_system.Service;

namespace Online_food_delivery_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowReactApp")]
    public class TokenController : ControllerBase
    {
        private readonly FoodDbContext _context;
        private readonly ITokenGenerate _tokenService;

        public TokenController(FoodDbContext context, ITokenGenerate tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }


    [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Email and password are required.");
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginRequest.Email && u.Password == loginRequest.Password);

            if (user == null)
            {
                return Unauthorized("Invalid credentials.");
            }

            var token = _tokenService.GenerateToken(user);
            return Ok(new { token=token,
             role = user.Role
            });
        }

}
}
