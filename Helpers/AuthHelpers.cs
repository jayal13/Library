using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Library.Helpers
{
    public class AuthHelper(IConfiguration config)
    {
        public byte[] GetPasswordHash(string password, byte[] passwordSalt)
        {
            return KeyDerivation.Pbkdf2(
                password,
                passwordSalt,
                KeyDerivationPrf.HMACSHA256,
                100, 256 / 8);
        }
        public string CreateToken(int userId)
        {
            Claim[] claims = [
                new Claim("userId", userId.ToString())
            ];
            string? tokenkeyString = config.GetSection("AppSettings:TokenKey").Value;

            SymmetricSecurityKey tokenKey = new(
                Encoding.UTF8.GetBytes(tokenkeyString ?? ""));

            SigningCredentials credentials = new(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddDays(1)
            };

            JwtSecurityTokenHandler tokenHandler = new();

            SecurityToken token = tokenHandler.CreateToken(descriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}