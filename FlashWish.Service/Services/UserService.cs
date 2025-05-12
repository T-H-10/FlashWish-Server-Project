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
        public async Task<UserResponseDTO> AddUserAsync(UserDTO user)
        {
            var userToAdd = _mapper.Map<User>(user);
            if (userToAdd != null)
            {
                userToAdd.Password = BCrypt.Net.BCrypt.HashPassword(userToAdd.Password);
                userToAdd.CreatedAt = DateTime.UtcNow;
                userToAdd.UpdatedAt = DateTime.UtcNow;
                await _repositoryManager.Users.AddAsync(userToAdd);
                await _repositoryManager.SaveAsync();
                return _mapper.Map<UserResponseDTO>(userToAdd);
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

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _repositoryManager.Users.GetAllAsync();
            return _mapper.Map<IEnumerable<UserResponseDTO>>(users);
        }

        public async Task<UserResponseDTO?> GetUserByIdAsync(int id)
        {
            var user = await _repositoryManager.Users.GetByIdAsync(id);
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<IEnumerable<UserWithRolesDTO>> GetUsersWithRolesAsync()
        {
           return await _repositoryManager.Users.GetUsersWithRolesAsync();
        }

        public async Task<UserResponseDTO?> UpdateUserAsync(int id, UserDTO user)
        {
            if (user == null)
            {
                return null;
            }
            var userToUpdate = _mapper.Map<User>(user);
            userToUpdate.UpdatedAt = DateTime.UtcNow;
            await _repositoryManager.Users.UpdateAsync(id, userToUpdate);
            await _repositoryManager.SaveAsync();
            return _mapper.Map<UserResponseDTO?>(userToUpdate);
        }


        public async Task<UserResponseDTO?> GetUserByEmailAsync(string email)
        {
            var user = await _repositoryManager.Users.GetByEmailAsync(email); // הנח שיש לך פונקציה כזו
            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<bool> UpdateUserRoleAsynv(int id, string newRole)
        {
            var user = await _repositoryManager.Users.GetByIdAsync(id);
            if (user == null) return false;
            //user.Roles.Add(newRole);
            await _repositoryManager.Users.UpdateAsync(id, user);
            return true;
        }

        public async Task<bool> AddAdminRoleAsync(int userId)
        {
            return await _repositoryManager.Users.AddAdminRoleAsync(userId);
        }

        public async Task<bool> RemoveAdminRoleAsync(int userId)
        {
            return await _repositoryManager.Users.RemoveAdminRoleAsync(userId);
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
