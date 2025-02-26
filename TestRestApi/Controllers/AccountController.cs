using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TestRestApi.DATA.Models;
using TestRestApi.Models;

namespace TestRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public AccountController(UserManager<UserData> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            this.configuration = configuration;
        }
        private readonly UserManager<UserData> _userManager;
        private readonly IConfiguration configuration;

        [HttpPost("[Action]")]
        public async Task<IActionResult> Register(dtoNewUser user)
        {
            if (ModelState.IsValid)
            {
                UserData UserData = new()
                {
                    UserName = user.UserName,
                    Email = user.Email,

                };
                IdentityResult result = await _userManager.CreateAsync(UserData, user.UserName);
                if (result.Succeeded)
                {
                    return Ok("success");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        //    public async Task<IActionResult> LogIn(dtoLogin login)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }

        //        IdentityUser? user = await _userManager.FindByNameAsync(login.UserName) as UserData;

        //        if (user == null)
        //        {
        //            ModelState.AddModelError("", "Username is not found");
        //            return BadRequest(ModelState);
        //        }

        //        // Check password
        //        if (!await _userManager.CheckPasswordAsync((UserData)user, login.Password))
        //        {
        //            return Unauthorized();
        //        }

        //        // Generate claims
        //        var claims = new List<Claim>
        //{
        //    new Claim(ClaimTypes.Name, user.UserName),
        //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //};

        //        var roles = await _userManager.GetRolesAsync((UserData)user);
        //        foreach (var role in roles)
        //        {
        //            claims.Add(new Claim(ClaimTypes.Role, role));
        //        }

        //        // Generate JWT Token
        //        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"]));
        //        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //        var token = new JwtSecurityToken(
        //            issuer: configuration["JWT:Issuer"],
        //            audience: configuration["JWT:Audience"],
        //            claims: claims,
        //            expires: DateTime.UtcNow.AddHours(1),
        //            signingCredentials: creds
        //        );
        //        var _token = new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo,
        //        };

        //        // Return token
        //        return Ok(new
        //        {
        //            token = new JwtSecurityTokenHandler().WriteToken(token),
        //            expiration = token.ValidTo
        //        });
        //    }

        [HttpPost]
        public async Task<IActionResult> LogIn(dtoLogin login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Fix: Use UserData directly
            UserData? user = await _userManager.FindByNameAsync(login.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Username is not found");
                return BadRequest(ModelState);
            }

            // Fix: Remove unnecessary casting in password check
            if (await _userManager.CheckPasswordAsync(user, login.Password))
            {
                return Ok ("pass issyes");
            }

            // Generate claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Fix: Ensure JWT Secret Key is valid
            string secretKey = configuration["JWT:SecretKey"] ?? "default_secret";
            if (secretKey == "default_secret")
            {
                Console.WriteLine("Warning: JWT Secret Key is missing in appsettings.json!");
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            string generatedToken = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Generated Token: {generatedToken}");

            return Ok(new
            {
                token = generatedToken,
                expiration = token.ValidTo
            });
        }
    }

}
