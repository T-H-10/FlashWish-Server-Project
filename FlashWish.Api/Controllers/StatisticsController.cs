using FlashWish.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FlashWish.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;

        public StatisticsController(StatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }
        // GET: api/<StatisticsController>
        [HttpGet]
        public async Task<ActionResult> GetDashboardStats()
        {
            var stats = await _statisticsService.GetDashBoardStatistics();
            //    new
            //{
            //    UsersCount = 120,   // כמות המשתמשים הרשומים
            //    GreetingsCount = 340,  // כמות הברכות שנוצרו
            //    BackgroundsCount = 45,  // כמות הרקעים
            //    RecentActivities = new[]
            //    {
            //    "משתמש חדש נרשם למערכת",
            //    "נוספה ברכה חדשה",
            //    "משתמש עדכן תמונת רקע"
            //}
            //};
            return Ok(stats);
        }

        //    // GET api/<StatisticsController>/5
        //    [HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<StatisticsController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<StatisticsController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<StatisticsController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
