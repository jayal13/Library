using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Library.Data;
using Library.Dtos;
using Library.Helpers;
using Library.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthHelper authHelper;
        private readonly ILibraryRepository libraryRepository;

        public AuthController(ILibraryRepository repo, IConfiguration config)
        {
            libraryRepository = repo;
            authHelper = new(config);
        }
        
        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(RegistrationDto registration)
        {
            if (registration.Password != registration.PasswordConfirmation)
            {
                throw new Exception("PasswordHasher do NotFiniteNumberException match");
            }
            Auth? user = libraryRepository.GetOneBy<Auth>(a => a.Email == registration.Email);
            if (user != null)
            {
                throw new Exception("User with this Email already exists");
            }
            byte[] passwordSalt = new byte[128 / 8];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetNonZeroBytes(passwordSalt);
            }
            var passwordHash = authHelper.GetPasswordHash(registration.Password, passwordSalt);
            var loginConfirmation = new Auth
            {
                Email = registration.Email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            var rows = libraryRepository.AddOne<Auth>(loginConfirmation);
            if (rows != 1)
            {
                throw new Exception("Error creating the Account");
            }

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Auth(LoginDto login)
        {
            Auth? user = libraryRepository.GetOneBy<Auth>(a => a.Email == login.Email)
            ?? throw new KeyNotFoundException($"User {login.Email} not found");
            var passwordHash = authHelper.GetPasswordHash(login.Password, user.PasswordSalt);
            for (int i = 0; i < passwordHash.Length; i++)
            {
                if (passwordHash[i] != user.PasswordHash[i])
                {
                    return StatusCode(401, "Incorrect password");
                }
            }
            return Ok(new Dictionary<string, string>
            {
                {"token", authHelper.CreateToken(user.Id)}
            });
        }

        [HttpGet("RefreshToken")]
        public IActionResult RefreshToken()
        {
            string userId = User.FindFirst("userId")?.Value + "";
            return Ok(new Dictionary<string, string> { { "token", authHelper.CreateToken(int.Parse(userId)) } });
        }
    }
}