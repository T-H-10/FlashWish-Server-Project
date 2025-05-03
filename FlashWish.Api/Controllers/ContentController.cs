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

                var response = await _httpClient.PostAsJsonAsync(SERVER_URL, request);

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


        //[HttpPost("generate")]
        //public async Task<IActionResult> GenerateContent([FromBody] ContentRequest request)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(SERVER_URL, request);
        //        Console.WriteLine("------\nrequest:\n" + request.Prompt + "\n" + request.Style + "\n" + request.Rhyming + "\n" + request.Length + "\n" + request.RecipientGender);
        //        Console.WriteLine("------\nresponse:\n"+response);
        //        if (!response.IsSuccessStatusCode)
        //        {
        //            var error = await response.Content.ReadAsStringAsync();
        //            return StatusCode((int)response.StatusCode, error);
        //        }
        //        var aiResponse = await response.Content.ReadFromJsonAsync<ContentResponse>();
        //        Console.WriteLine(aiResponse);
        //        return Ok(aiResponse);
        //    }

        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { error = ex.Message });
        //    }

        //}
    }
}
