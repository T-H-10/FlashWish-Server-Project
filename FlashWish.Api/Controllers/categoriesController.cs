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
    public class categoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public categoriesController(ICategoryService categoryService, IMapper mapper)
        {
            _categoryService = categoryService;
            _mapper = mapper;
        }
        // GET: api/<categoriesController>
        [HttpGet]
        public async Task<ActionResult<CategoryDTO>> GetAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            if (categories == null)
            {
                return NotFound();//404
            }
            if (!categories.Any())
            {
                return NoContent();//204
            }
            return Ok(categories);//200
        }

        // GET api/<categoriesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDTO>> GetAsync(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound(id);//404
            }
            return Ok(category);//200
        }

        // POST api/<categoriesController>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> PostAsync([FromBody] CategoryPostModel category)
        {
            if (category == null)
            {
                return BadRequest();//400
            }
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            if (categoryDTO == null)
            {
                return BadRequest();
            }
            var createdCategory = await _categoryService.AddCategoryAsync(categoryDTO);
            if (createdCategory == null)
            {
                return BadRequest();//400
            }
            return Ok(createdCategory);//200
        }

        // PUT api/<categoriesController>/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CategoryDTO>> PutAsync(int id, [FromBody] CategoryPostModel category)
        {
            if (category == null)
            {
                return BadRequest();//400
            }
            var categoryDTO = _mapper.Map<CategoryDTO>(category);
            var updatedCategory = await _categoryService.UpdateCategoryAsync(id, categoryDTO);
            if (updatedCategory == null)
            {
                return NotFound();//404
            }
            return Ok(updatedCategory);//200
        }

        // DELETE api/<categoriesController>/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int id)
        {
            var isDeleted = await _categoryService.DeleteCategoryAsync(id);
            if (!isDeleted)
            {
                return NotFound();//404
            }
            return NoContent();//204
        }
    }
}
