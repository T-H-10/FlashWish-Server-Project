using DotNetEnv;
using FlashWish.Core.Entities;
using Microsoft.AspNetCore.Mvc;
//using System.Web.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        public ContentController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateContent([FromBody] ContentRequest request)
        {
            Env.Load();
            var SERVER_URL = Env.GetString("SERVER_AI_URL");

            try
            {
                Console.WriteLine($"""
        ----- REQUEST RECEIVED -----
        Prompt: {request.Prompt}
        Style: {request.Style}
        Rhyming: {request.Rhyming}
        Length: {request.Length}
        Recipient Gender: {request.RecipientGender}
        Important Words: {string.Join(", ", request.ImportantWords)}
        ----------------------------
        """);

                var response = await _httpClient.PostAsJsonAsync(SERVER_URL+"/generate", request);
                Console.WriteLine(response.Content.ToString());
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return StatusCode((int)response.StatusCode, error);
                }
                var aiResponse = await response.Content.ReadFromJsonAsync<ContentResponse>();
                return Ok(aiResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

    }
}
