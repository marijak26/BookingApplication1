using Booking.Domain.DTO;
using Booking.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Booking.Web.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<BookingApplicationUser> _userManager;
        private readonly string _secretKey;

        public AuthController(UserManager<BookingApplicationUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _secretKey = config["Jwt:Key"];
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    return Unauthorized(new 
                    { 
                        message = "Invalid credentials" 
                    });
                }

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Email),
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                return Ok(new 
                { 
                    token = new JwtSecurityTokenHandler().WriteToken(token) 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new 
                { 
                    message = ex.Message, stack = ex.StackTrace 
                });
            }
        }
    }
}
