using AutoMapper;
using FlashWish.Api.PostModels;
using FlashWish.Core.DTOs;
using FlashWish.Core.IServices;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IConfiguration configuration, IAuthService authService, IMapper mapper)
        {
            _configuration = configuration;
            _authService = authService;
            _mapper = mapper;
        }
        [HttpPost("login")]
        public async Task<ActionResult<LoginResultDTO>> LoginAsync([FromBody] LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.Email) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return BadRequest("Email and password are required.");//400
            }
            var user = await _authService.LoginAsync(loginModel.Email, loginModel.Password);
            if (user == null)
            {
                return Unauthorized();//401
            }
            return Ok(user);//200
        }

        [HttpPost("register")]
        public async Task<ActionResult<LoginResultDTO>> RegisterAsync([FromBody] UserPostModel userPostModel)
        {
            if (string.IsNullOrWhiteSpace(userPostModel.Email) || string.IsNullOrWhiteSpace(userPostModel.Password))
            {
                return BadRequest("Email and password are required.");//400
            }
            //var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userPostModel.Password);
            var userDto = _mapper.Map<UserDTO>(userPostModel);
            var userRegistered = await _authService.RegisterAsync(userDto);
            if (userRegistered == null)
            {
                return Conflict("User already exist."); //409
            }
            return Ok(userRegistered);
        }

        
    }
}
