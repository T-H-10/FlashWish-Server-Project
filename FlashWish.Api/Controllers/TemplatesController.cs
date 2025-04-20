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
    public class TemplatesController : ControllerBase
    {
        private readonly ITemplateService _templateService;
        private readonly IMapper _mapper;
        public TemplatesController(ITemplateService templateService, IMapper mapper)
        {
            _templateService = templateService;
            _mapper = mapper;
        }
        // GET: api/<TemplatesController>
        [HttpGet]
        public async Task<ActionResult<TemplateDTO>> GetAsync()
        {
            var templates = await _templateService.GetAllTemplatesAsync();
            if (templates == null)
            {
                return NotFound(); //404
            }
            if (!templates.Any())
            {
                return NoContent(); //204
            }
            return Ok(templates);//200
        }

        // GET api/<TemplatesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TemplateDTO>> GetAsync(int id)
        {
            var template = await _templateService.GetTemplateByIdAsync(id);
            if (template == null)
            {
                return NotFound(id);//404
            }

            return Ok(template);//200
        }

        // POST api/<TemplatesController>
        [HttpPost]
        //[Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult<TemplateDTO>> PostAsync([FromForm] TemplatePostModel template)
        {
            if (template == null || template.ImageFile == null || template.ImageFile.Length <= 0)
            {
                return BadRequest();//400
            }
            var templateDTO = _mapper.Map<TemplateDTO>(template);
            if (templateDTO == null)
            {
                return NotFound();//404
            }
            var createdTemplate = await _templateService.AddTemplateAsync(templateDTO, template.ImageFile);
            if (createdTemplate == null)
            {
                return BadRequest("You should give a uniqe name to your template. maybe this is your problam");//400
            }
            return Ok(createdTemplate);//200
        }

        // PUT api/<TemplatesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<TemplateDTO>> PutAsync(int id, [FromBody] TemplatePostModel template)
        {
            //if (template == null)
            //{
            //    return BadRequest();//400
            //}
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Console.WriteLine(User.IsInRole("Admin"));
            if (currentUserId != template.UserID.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid(); // 403
            }
            var templateDTO = _mapper.Map<TemplateDTO>(template);
            templateDTO = await _templateService.UpdateTemplateAsync(id, templateDTO);
            if (templateDTO == null)
            {
                return NotFound();
            }
            return Ok(templateDTO);//200
        }

        // DELETE api/<TemplatesController>/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "EditorOrAdmin")]
        public async Task<ActionResult> DeleteAsync(int id)
        {
            //var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (currentUserId != id.ToString() && !User.IsInRole("Admin"))
            //{
            //    return Forbid(); // 403 - אין הרשאה
            //}
            var isDeleted = await _templateService.DeleteTemplateAsync(id);
            if (!isDeleted)
            {
                return NotFound();//404
            }
            return NoContent();//204

        }
    }
}
