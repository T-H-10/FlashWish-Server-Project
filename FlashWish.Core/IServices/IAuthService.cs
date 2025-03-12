using FlashWish.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Core.IServices
{
    public interface IAuthService
    {
        public Task<bool> ValidateUserAsync(string email, string password);
        public Task<LoginResultDTO> LoginAsync(string userEmail, string password);
        public Task<LoginResultDTO> RegisterAsync(UserDTO userDto);

    }
}
