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
        private readonly CreteToken _creteToken;

        public AuthService(AuthRepository repository, IConfiguration configuration, CreteToken creteToken)
        {
            _repository = repository;
            _configuration = configuration;
            _creteToken = creteToken;
        }

        public async Task<User> RegisterUserAsync(User user,string token)
        {
            if(token==null) throw new Exception("Bu email ile kayıtlı bir kullanıcı zaten mevcut.");
            
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

            return _creteToken.GenerateToken(user);
        }


    }
}