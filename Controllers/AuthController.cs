using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RegistryRecord.DTOs;
using RegistryRecord.Entities;
using RegistryRecord.Services;

namespace RegistryRecord.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("add-user")]
        [Authorize]
        public async Task<IActionResult> AddUser([FromBody] AddUserDto dto)
        {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            
            try
            {
                var user = new User
                {
                    UserName = dto.UserName,
                    Password = dto.Password,
                    Email = dto.Email,
                    FirstName = dto.FirstName,
                    LastName = dto.LastName
                };

                var createdUser = await _authService.RegisterUserAsync(user,token); 

                return Ok(new
                {
                    createdUser.Id,
                    createdUser.UserName,
                    createdUser.Email,
                    createdUser.FirstName,
                    createdUser.LastName
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            try
            {
                var token = await _authService.LoginAsync(dto.Email, dto.Password);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}