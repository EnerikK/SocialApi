using Microsoft.AspNetCore.Mvc;

namespace Social.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id,CancellationToken cancellationToken)
        {
            return Ok();
        }
    }
}