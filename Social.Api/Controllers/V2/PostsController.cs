using Microsoft.AspNetCore.Mvc;
using Social.Domain.Models;

namespace Social.Api.Controllers.V2
{
    [ApiVersion("2.0")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class PostsController : Controller
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(int id)
        {
            var post = new Post { Id = id, Text = "Hello,Enerik!" };
            return Ok(post);
        }
    }
}