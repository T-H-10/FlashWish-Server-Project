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
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO?> GetUserByIdAsync(int id);
        Task<UserDTO> AddUserAsync(UserDTO user);
        Task<UserDTO?> UpdateUserAsync(int id, UserDTO user);
        Task<bool> DeleteUserAsync(int id);
        //Task<UserDTO?> GetUserByEmailAsync(string email);

        Task<bool> UserEmailExistsAsync(string email);
        //Task<string> CreateUserAsync(string email, string password);

    }
}
