using RegistryRecord.Entities;
using RegistryRecord.Repositories;
using RegistryRecord.Helpers;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RegistryRecord.Services
{
    public class AuthService
    {
        private readonly AuthRepository _repository;
        private readonly IConfiguration _configuration;

        public AuthService(AuthRepository repository, IConfiguration configuration)
        {
            _repository = repository;
            _configuration = configuration;
        }

        public async Task<User> RegisterUserAsync(User user,string token)
        {
            if (await _repository.UserExistsAsync(user.Email))
                throw new Exception("Bu email ile kayıtlı bir kullanıcı zaten mevcut.");

            user.Password = CreatePassword.HashPassword(user.Password);

            return await _repository.AddUserAsync(user);
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _repository.GetUserByEmailAsync(email);
            if (user == null) throw new Exception("Kullanıcı bulunamadı.");
            

            if (!CreatePassword.VerifyPassword(password, user.Password))
                throw new Exception("Şifre yanlış.");

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}