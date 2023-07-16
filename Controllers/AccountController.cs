using AspNet_Core6.Fundamentals.Data;
using AspNet_Core6.Fundamentals.Extensions;
using AspNet_Core6.Fundamentals.Models;
using AspNet_Core6.Fundamentals.Services.Interfaces;
using AspNet_Core6.Fundamentals.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace AspNet_Core6.Fundamentals.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly BlogDataContext _blogDataContext;

        public AccountController(ITokenService tokenService, BlogDataContext blogDataContext)
        {
            _tokenService = tokenService;
            _blogDataContext = blogDataContext;
        }

        [HttpPost("v1/account")]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = new User
            {
                Name = registerViewModel.Name,
                Email = registerViewModel.Email,
                Slug = registerViewModel.Email.Replace("@", "-").Replace(".", "-")
            };

            var password = PasswordGenerator.Generate(length: 25);
            user.PasswordHash = PasswordHasher.Hash(password);

            try
            {
                await _blogDataContext.AddAsync(user);
                await _blogDataContext.SaveChangesAsync();

                return Ok(new ResultViewModel<dynamic>(new
                {
                    user = user.Email,
                    password
                }));
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(400, new ResultViewModel<string>(ex.ToString()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>(ex.ToString()));
            }
        }

        [HttpPost("v1/account/login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

            var user = await _blogDataContext.Users
                .AsNoTracking()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == loginViewModel.Email);

            if (user == null)
                return StatusCode(401, new ResultViewModel<string>(error: "User ou password invalid"));

            if (!PasswordHasher.Verify(user.PasswordHash, loginViewModel.Password))
                return StatusCode(401, new ResultViewModel<string>(error: "User ou password invalid"));

            try
            {
                var token = _tokenService.GenerateToken(user);
                return Ok(token);
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(400, new ResultViewModel<string>(ex.ToString()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<string>(ex.ToString()));
            }
        }
    }
}
