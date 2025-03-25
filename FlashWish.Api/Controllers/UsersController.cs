using AutoMapper;
using FlashWish.Api.PostModels;
using FlashWish.Core.DTOs;
using FlashWish.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
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
        [Authorize(Roles = "Admin")]
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
        //[Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<UserDTO>> PutAsync(int id, [FromBody] UserPostModel user)
        {
            if (user == null)
            {
                return BadRequest();//400
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != id.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403 - אין הרשאה
            }
            var userDTO = _mapper.Map<UserDTO>(user);
            var updatedUser = await _userService.UpdateUserAsync(id, userDTO);
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
    }
}
