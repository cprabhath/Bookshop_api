using Bookshop_api.Data;
using Bookshop_api.Models;
using Bookshop_api.Validations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Bookshop_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ApplicationDBContext _context;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration, ApplicationDBContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("customer-register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegistration model)
        {
            var validations = new CustomerValidationValidator();
            var validationResult = validations.Validate(new CustomerValidations(
                model.Email, model.Password, model.Name, model.MobileNumber, model.Address
            ));

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var customer = new IdentityUser { UserName = model.Email };
            var result = await _userManager.CreateAsync(customer, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("User"));
                }
                await _userManager.AddToRoleAsync(customer, "User");

                var newCustomer = new Customer
                {
                    Name = model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    Address = model.Address,
                    ReadingGoals = model.ReadingGoals,
                    FavoriteGenres = model.FavoriteGenres,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now
                };
                await _context.Customers.AddAsync(newCustomer);
                await _context.SaveChangesAsync();

                return Ok(new { message = "User created successfully", Username = customer.UserName, Role = "User" });
            }
            return BadRequest(result.Errors);
        }


        [HttpPost("admin-register")]
        public async Task<IActionResult> RegisterAdmin([FromBody] Login model)
        {
            var admin = new IdentityUser { UserName = model.Email };
            var result = await _userManager.CreateAsync(admin, model.Password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    await _roleManager.CreateAsync(new IdentityRole("Admin"));
                }
                await _userManager.AddToRoleAsync(admin, "Admin");

                return Ok(new { message = "Admin created successfully", Username = admin.UserName, Role = "Admin" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("password-reset")]
        public async Task<IActionResult> ResetPassword([FromBody] Login model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (result.Succeeded)
                {
                    return Ok(new { message = "Password reset successfully" });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("User not found");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            var user  = await _userManager.FindByNameAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                authClaims.AddRange(role.Select(r => new Claim(ClaimTypes.Role, r)));

                var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"]!)),
                        claims: authClaims,
                        signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!)),
                        SecurityAlgorithms.HmacSha256)
                    );

                Customer? customerDetails = null;

                if (role.Contains("User"))
                {
                    customerDetails = await _context.Customers.FirstOrDefaultAsync(
                        c => c.Email == model.Email
                    );
                }

                return Ok(new {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    User = customerDetails
                }); 
            }

            return BadRequest("Invalid Username or Password");
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromBody] string role)
        {
            if(!await _roleManager.RoleExistsAsync(role))
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(role));
                if (result.Succeeded) { 
                    return Ok(new { message = "Role created successfully" });
                }
                return BadRequest(result.Errors);
            }
            return BadRequest("Role already exists");
        }


    }
}
