using Microsoft.AspNetCore.Mvc;
using Social.Domain.Models;

namespace Social.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var post = new Post { Id = id, Text = "Hello,World!" };
            return Ok(post);
        }
    }
}
