using AutoMapper;
using FlashWish.Core.DTOs;
using FlashWish.Core.Entities;
using FlashWish.Core.IRepositories;
using FlashWish.Core.IServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashWish.Service.Services
{
    public class UserService:IUserService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public UserService(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }
        public async Task<UserDTO> AddUserAsync(UserDTO user)
        {
            var userToAdd = _mapper.Map<User>(user);
            if (userToAdd != null)
            {
                userToAdd.CreatedAt = DateTime.UtcNow;
                userToAdd.UpdatedAt = DateTime.UtcNow;
                await _repositoryManager.Users.AddAsync(userToAdd);
                await _repositoryManager.SaveAsync();
                return _mapper.Map<UserDTO>(userToAdd);
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var userDTO = await _repositoryManager.Users.GetByIdAsync(id);
            if (userDTO == null)
            {
                return false;
            }
            //var userToDelete = _mapper.Map<User>(userDTO);
            //if (userToDelete == null)
            //{
            //    return false;
            //}
            await _repositoryManager.Users.DeleteAsync(userDTO);
            await _repositoryManager.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        {
            var users = await _repositoryManager.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO?> GetUserByIdAsync(int id)
        {
            var user = await _repositoryManager.Users.GetByIdAsync(id);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserWithRolesDTO>> GetUsersWithRolesAsync()
        {
           return await _repositoryManager.Users.GetUsersWithRolesAsync();
        }

        public async Task<UserDTO?> UpdateUserAsync(int id, UserDTO user)
        {
            if (user == null)
            {
                return null;
            }
            var userToUpdate = _mapper.Map<User>(user);
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Users.UpdateAsync(id, userToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<UserDTO?>(userToUpdate);
        }


        public async Task<UserDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _repositoryManager.Users.GetByEmailAsync(email); // הנח שיש לך פונקציה כזו
            return _mapper.Map<UserDTO>(user);
        }

        //public async Task<bool> UserEmailExistsAsync(string email)
        //{
        //    return await _repositoryManager.Users.UserEmailExistsAsync(email);
        //}
        //public async Task<string> CreateUserAsync(string email, string password)
        //{
        //    var userID = await _repositoryManager.Users.CreateUserAsync(email, password);
        //    return userID;
        //}
    }
}
