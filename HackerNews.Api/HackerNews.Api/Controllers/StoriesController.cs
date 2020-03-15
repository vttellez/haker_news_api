using System.Threading.Tasks;
using HackerNews.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("MyAllowSpecificOrigins")]
    public class StoriesController : ControllerBase
    {

        private readonly IHackerNewsService _hackernewsService;
        public StoriesController(IHackerNewsService hackernewsService)
        {
            _hackernewsService = hackernewsService;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        [Route("{page=1}/{count=10}")]
        public async Task<IActionResult> Get(short page = 1, short count = 10)
        {
            try
            {
                return Ok(await _hackernewsService.GetNewStories(page, count));
            }
            catch  //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}
