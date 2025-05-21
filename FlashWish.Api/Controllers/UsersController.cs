using AutoMapper;
using FlashWish.Api.PostModels;
using FlashWish.Core.DTOs;
using FlashWish.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // GET: api/<UsersController>
        [HttpGet]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> GetAsync()
        {
            var users = await _userService.GetAllUsersAsync();
            if (users == null)
            {
                return NotFound();//404
            }
            if (!users.Any())
            {
                return NoContent();//204
            }
            return Ok(users);//200
        }

        [HttpGet("Roles")]
        [Authorize(Roles ="Admin")]
        public async Task<ActionResult<UserWithRolesDTO>> GetUsersWithRoles()
        {
            var users = await _userService.GetUsersWithRolesAsync();
            if (users == null)
            {
                return NotFound(); //404
            }
            if (!users.Any())
            {
                return NoContent(); //204
            }
            return Ok(users); //200
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        //[Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<UserDTO>> GetAsync(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound(id);//404
            }
            return Ok(user);//200
        }

        // POST api/<UsersController>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserDTO>> PostAsync([FromBody] UserPostModel user)
        {
            if (user == null)
            {
                return BadRequest();//400
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            var createdUser = await _userService.AddUserAsync(userDTO);
            if (createdUser == null)
            {
                return BadRequest();//400
            }
            return Ok(createdUser);//200
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        [Authorize(Policy = "EditorOrAdmin")]
        public async Task<ActionResult<UserDTO>> PutAsync(int id, [FromBody] UserPostModel user)
        {
            if (user == null)
            {
                return BadRequest();//400
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //Console.WriteLine(currentUserId);
            //Console.WriteLine(id);
            //Console.WriteLine(User.IsInRole("Admin"));
            if (currentUserId != id.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403 - אין הרשאה
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            var updatedUser = await _userService.UpdateUserAsync(id, userDTO);
            updatedUser.Id = id; // fix it!!!!----
            //Console.WriteLine(updatedUser.Id + "<<-------------");
            if (updatedUser == null)
            {
                return NotFound();//404
            }
            return Ok(updatedUser);//200
        }


        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var isDeleted = await _userService.DeleteUserAsync(id);
            if (!isDeleted)
            {
                return NotFound(); // 404
            }
            return NoContent(); // 204
        }

        [HttpPut("{id}/add-admin-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddAdminRole(int id)
        {
            var result = await _userService.AddAdminRoleAsync(id);
            if (!result)
            {
                return NotFound(); // 404
            }
            return NoContent(); // 204
        }

        [HttpPut("{id}/remove-admin-role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> RemoveAdminRole(int id)
        {
            var result = await _userService.RemoveAdminRoleAsync(id);
            if (!result)
            {
                return NotFound(); // 404
            }
            return NoContent(); // 204
        }

        [HttpGet("email-exists")]
        public async Task<ActionResult<UserResponseDTO?>> UserEmailExistsAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email))
            {
                return BadRequest("Invalid email format"); //400
            }
            var exists = await _userService.GetUserByEmailAsync(email);
            return exists; //200 , 204
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
