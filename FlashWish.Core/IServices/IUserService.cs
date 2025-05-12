using FlashWish.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task<IEnumerable<UserWithRolesDTO>> GetUsersWithRolesAsync();
        Task<UserResponseDTO?> GetUserByIdAsync(int id);
        Task<UserResponseDTO> AddUserAsync(UserDTO user);
        Task<UserResponseDTO?> UpdateUserAsync(int id, UserDTO user);
        Task<bool> DeleteUserAsync(int id);
        Task<UserResponseDTO?> GetUserByEmailAsync(string email);
        Task<bool> AddAdminRoleAsync(int userId);
        Task<bool> RemoveAdminRoleAsync(int userId);

        //Task<bool> UserEmailExistsAsync(string email);
        //Task<string> CreateUserAsync(string email, string password);

    }
}
