using FlashWish.Core.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private const string SERVER_URL = "http://localhost:5001/generate";
        public ContentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] ContentRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(SERVER_URL, new { prompt = request.Prompt });
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, error);
                }
                var aiResponse = await response.Content.ReadFromJsonAsync<ContentResponse>();
                Console.WriteLine(aiResponse);
                return Ok(aiResponse);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }

            //    // GET: api/<ContentController>
            //    [HttpGet]
            //    public IEnumerable<string> Get()
            //    {
            //        return new string[] { "value1", "value2" };
            //    }

            //    // GET api/<ContentController>/5
            //    [HttpGet("{id}")]
            //    public string Get(int id)
            //    {
            //        return "value";
            //    }

            //    // POST api/<ContentController>
            //    [HttpPost]
            //    public void Post([FromBody]string value)
            //    {
            //    }

            //    // PUT api/<ContentController>/5
            //    [HttpPut("{id}")]
            //    public void Put(int id, [FromBody]string value)
            //    {
            //    }

            //    // DELETE api/<ContentController>/5
            //    [HttpDelete("{id}")]
            //    public void Delete(int id)
            //    {
            //    }
            //}
        }
    }
}
