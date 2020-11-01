using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using NetCore.Data.Entities;
using NetCore.WebApi.Models;

namespace NetCore.WebApi.Controllers
{

    public class AccountController : ApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public AccountController(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ILogger<AccountController> logger, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _config = config;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult("Username or Password Invalid!");
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, true);
                if (!result.Succeeded)
                {
                    return new BadRequestObjectResult("Account not found!");
                }
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim("fullName", user.FullName),
                    new Claim("avatar", string.IsNullOrEmpty(user.Avatar)? string.Empty:user.Avatar),
                    new Claim("roles", string.Join(";",roles)),
                    new Claim("permissions",""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                _logger.LogError(_config["Tokens"]);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(_config["Tokens:Issuer"],
                    _config["Tokens:Issuer"],
                    claims,
                    expires: DateTime.UtcNow.AddMinutes(30),
                    signingCredentials: creds);
                _logger.LogInformation(1, "User logged in.");

                return new OkObjectResult(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
            }
            return new BadRequestObjectResult("Systen error!");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(model);
            }

            var user = new AppUser { FullName = "demo", UserName = model.Email, Email = model.Email };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // User claim for write customers data
                await _userManager.AddClaimAsync(user, new Claim("Customers", "Write"));

                await _signInManager.SignInAsync(user, false);
                _logger.LogInformation(3, "User created a new account with password.");
                return new OkObjectResult(model);
            }

            return new BadRequestObjectResult(model);
        }
    }
}