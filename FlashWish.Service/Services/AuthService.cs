using AutoMapper;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlashWish.Service.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public AuthService(IConfiguration configuration, IRepositoryManager repositoryManager, IMapper mapper)
        {
            _configuration = configuration;
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        protected string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.RoleName));
            }

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(5),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            User user = await _repositoryManager.Users.GetByEmailAsync(email);
            return user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public async Task<LoginResultDTO> LoginAsync(string email, string password)
        {
            //if (await ValidateUserAsync(email, password))
            var user = await _repositoryManager.Users.GetByEmailAsync(email);
            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                var token = GenerateJwtToken(user);
                var userDTO = _mapper.Map<UserDTO>(user);
                return new LoginResultDTO
                {
                    User = userDTO,
                    Token = token
                };
            }
            return null;
        }

        public async Task<LoginResultDTO> RegisterAsync(UserDTO userDto)
        {
            var currentUser = await _repositoryManager.Users.GetByEmailAsync(userDto.Email);
            if (currentUser != null)
            {
                return null;
            }

            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Roles = new List<Role>() { new Role { RoleName = "Editor" } }
            };

            var result = await _repositoryManager.Users.AddAsync(user);
            if (result == null)
            {
                return null;
            }

            await _repositoryManager.SaveAsync();

            var token = GenerateJwtToken(result);
            var resultUserDto = _mapper.Map<UserDTO>(result);

            return new LoginResultDTO
            {
                User = resultUserDto,
                Token = token
            };
        }
    }
}
