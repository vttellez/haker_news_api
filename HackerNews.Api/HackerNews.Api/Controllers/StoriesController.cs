using System.Threading.Tasks;
using HackerNews.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HackerNews.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<IActionResult> Get()
        {
            try
            {
                return Ok(await _hackernewsService.GetHackerNews());
            }
            catch  //(Exception ex)
            {
                //Add loggin functionality here to log the stack trace for support purposes  ex

                return StatusCode(StatusCodes.Status500InternalServerError, "Server Error");
            }
        }
    }
}
