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
    public class GreetingMessagesController : ControllerBase
    {
        private readonly IGreetingMessageService _greetingMessageService;
        private readonly IMapper _mapper;
        public GreetingMessagesController(IGreetingMessageService greetingMessageService, IMapper mapper)
        {
            _greetingMessageService = greetingMessageService;
            _mapper = mapper;
        }
        // GET: api/<GreetingMessagesController>
        [HttpGet]
        public async Task<ActionResult<GreetingMessageDTO>> GetAsync()
        {
            var greetingMessages = await _greetingMessageService.GetAllMessagesAsync();
            if (greetingMessages == null)
            {
                return NotFound();//404
            }
            if (!greetingMessages.Any())
            {
                return NoContent();//204
            }
            return Ok(greetingMessages);//200
        }

        // GET api/<GreetingMessagesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GreetingMessageDTO>> GetAsync(int id)
        {
            var greetingMessage = await _greetingMessageService.GetGreetingMessageByIdAsync(id);
            if (greetingMessage == null)
            {
                return NotFound(id); //404
            }
            return Ok(greetingMessage);//200
        }

        // POST api/<GreetingMessagesController>
        [HttpPost]
        //[Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingMessageDTO>> PostAsync([FromBody] GreetingMessagePostModel message)
        {
            if (message == null)
            {
                return BadRequest();//400
            }
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (currentUserId != message.UserID.ToString() && !User.IsInRole("Admin"))
            //{
            //    return Forbid(); // 403 - אין הרשאה
            //}
            var greetingDTO = _mapper.Map<GreetingMessageDTO>(message);
            var createdGreeting = await _greetingMessageService.AddGreetingMessageAsync(greetingDTO);
            if (greetingDTO == null)
            {
                return BadRequest(); //400
            }
            return Ok(createdGreeting);//200
        }

        // PUT api/<GreetingMessagesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingMessageDTO>> PutAsync(int id, [FromBody] GreetingMessagePostModel message)
        {
            if (message == null)
            {
                return BadRequest();//400
            }
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != message.UserID.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403
            }
            var greetingDTO = _mapper.Map<GreetingMessageDTO>(message);
            var updatedGreeting = await _greetingMessageService.UpdateGreetingMessageAsync(id, greetingDTO);
            if (updatedGreeting == null)
            {
                return NotFound(); //404
            }
            return Ok(updatedGreeting);//200
        }

        // DELETE api/<GreetingMessagesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != id.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403 - אין הרשאה
            }
            var isDeleted = await _greetingMessageService.DeleteGreetingMessageAsync(id);
            if (!isDeleted)
            {
                return NotFound(); //404
            }
            return NoContent();//204
        }
    }
}
