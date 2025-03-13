using AutoMapper;
using FlashWish.Api.PostModels;
using FlashWish.Core.DTOs;
using FlashWish.Core.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GreetingCardsController : ControllerBase
    {
        private readonly IGreetingCardService _greetingCardService;
        private readonly IMapper _mapper;

        public GreetingCardsController(IGreetingCardService greetingCardService, IMapper mapper)
        {
            _greetingCardService = greetingCardService;
            _mapper = mapper;
        }
        // GET: api/<GreetingCardsController>
        [HttpGet]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingCardDTO>> GetAsync()
        {
            var greetingCards = await _greetingCardService.GetAllGreetingCardsAsync();
            if (greetingCards == null)
            {
                return NotFound(); //404
            }
            if (!greetingCards.Any())
            {
                return NoContent();//204
            }
            return Ok(greetingCards);//200
        }
        // GET api/<GreetingCardsController>/5
        [HttpGet("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingCardDTO>> GetAsync(int id)
        {
            var greetingCards = await _greetingCardService.GetGreetingCardByIdAsync(id);
            if (greetingCards == null)
            {
                return NotFound(id);//404
            }
            return Ok(greetingCards);//200
        }

        // POST api/<GreetingCardsController>
        [HttpPost]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingCardDTO>> PostAsync([FromBody] GreetingCardPostModel greetingCard)
        {
            if (greetingCard == null)
            {
                return BadRequest();//400
            }
            var greetingDTO = _mapper.Map<GreetingCardDTO>(greetingCard);
            var createdCard = await _greetingCardService.AddGreetingCardAsync(greetingDTO);
            if (createdCard == null)
            {
                return BadRequest(); //400
            }
            return Ok(createdCard);//200
        }
        // PUT api/<GreetingCardsController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<GreetingCardDTO>> PutAsync(int id, [FromBody] GreetingCardPostModel greetingCard)
        {
            if (greetingCard == null)
            {
                return BadRequest();//400
            }
            var greetingDTO = _mapper.Map<GreetingCardDTO>(greetingCard);
            var updatedGreeting = await _greetingCardService.UpdateGreetingCardAsync(id, greetingDTO);
            if (updatedGreeting == null)
            {
                return NotFound(); //404
            }
            return Ok(updatedGreeting);//200
        }

        // DELETE api/<GreetingCardsController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            var isDeleted = await _greetingCardService.DeleteGreetingCardAsync(id);
            if (!isDeleted)
            {
                return NotFound();//404
            }
            return NoContent();//204
        }
    }
}
