using System.Reflection.Metadata;
using System.Security.Claims;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Social.Api.Contracts.Common;
using Social.Api.Contracts.Posts.Requests;
using Social.Api.Contracts.Posts.Responses;
using Social.Api.Extensions;
using Social.Api.Filters;
using Social.Application_UseCases_.Posts.Commands;
using Social.Application_UseCases_.Posts.Queries;
using Social.Domain.Aggregates.PostAggregate;
using Social.Domain.Validators.PostValidators;
using PostInteraction = Social.Domain.Aggregates.PostAggregate.PostInteraction;

namespace Social.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route(ApiRoutes.BaseRoute)]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PostsController : BaseController
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public PostsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetAllPosts(),cancellationToken);
            var map = _mapper.Map<List<PostResponse>>(result.PayLoad);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(map);
        }
        [HttpGet]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> GetById(string id ,CancellationToken cancellationToken)
        {
            var postId = Guid.Parse(id);
            var query = new GetPostById() { PostId = postId };
            var result = await _mediator.Send(query,cancellationToken);
            var map = _mapper.Map<PostResponse>(result.PayLoad);
            return result.IsError ? HandleErrorResponse(result.Errors) : Ok(map);
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> CreatePost([FromBody] PostCreate newPost ,CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userProfileId = identity?.FindFirst("UserProfileId")?.Value;
            var command = new CreatePost()
            {
                UserProfileId = Guid.Parse(userProfileId),
                TextContent = newPost.TextContent
            };
            var result = await _mediator.Send(command,cancellationToken);
            var map = _mapper.Map<PostResponse>(result.PayLoad);

            return result.IsError ? HandleErrorResponse(result.Errors) 
                : CreatedAtAction(nameof(GetById), new { id = result.PayLoad.UserProfileId }, map);

        }
        [HttpPatch]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePostText([FromBody] PostUpdate updatedPost, string id,CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userProfileId = identity?.FindFirst("UserProfileId")?.Value;
            var command = new UpdatePostText()
            {
                NewText = updatedPost.text,
                PostId = Guid.Parse(id),
                UserProfileId = Guid.Parse(userProfileId)
            };
            var result = await _mediator.Send(command,cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }
        [HttpDelete]
        [Route(ApiRoutes.Posts.IdRoute)]
        [ValidateGuid("id")]
        public async Task<IActionResult> DeletePost(string id,CancellationToken cancellationToken)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var userProfileId = identity?.FindFirst("UserProfileId")?.Value;
            var command = new DeletePost()
            {
                PostId = Guid.Parse(id),
                UserProfileId = Guid.Parse(userProfileId)
            };
            var result = await _mediator.Send(command,cancellationToken);

            return result.IsError ? HandleErrorResponse(result.Errors) : NoContent();
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.PostComment)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetCommentByPostId(string postId,CancellationToken cancellationToken)
        {
            var query = new GetPostComments()
            {
                PostId = Guid.Parse(postId)
            };
            var result = await _mediator.Send(query,cancellationToken);

            if (result.IsError) HandleErrorResponse(result.Errors);
            var comments = _mapper.Map<List<PostCommentResponse>>(result.PayLoad);
            return Ok(comments);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.PostComment)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddCommentToPost(string postId, [FromBody] PostCommentCreate comment,CancellationToken cancellationToken)
        {
            var isValidGuid = Guid.TryParse(comment.UserProfileId, out var userProfileId);
            
            if (!isValidGuid)
            {
                var apiError = new ErrorResponse();

                apiError.StatusCode = 1;
                apiError.StatusMessage = "Wrong Request";
                apiError.TimeStamp = DateTime.Now;
                apiError.Errors.Add("The Provided user profile id is not in a guid format");
                return BadRequest(apiError);
            }

            var command = new AddPostComment()
            {
                PostId = Guid.Parse(postId),
                UserProfileId = userProfileId,
                CommentText = comment.text
            };

            var result = await _mediator.Send(command,cancellationToken);
            if (result.IsError) return HandleErrorResponse(result.Errors);
            var newComment = _mapper.Map<PostCommentResponse>(result.PayLoad);
            return Ok(newComment);
        }

        [HttpGet]
        [Route(ApiRoutes.Posts.AddInteraction)]
        [ValidateGuid("postId")]
        public async Task<IActionResult> GetPostInteraction(string postId, CancellationToken token)
        {
            var postGuid = Guid.Parse(postId);
            var query = new GetPostInteractions() { PostId = postGuid };
            var result = await _mediator.Send(query, token);

            if (result.IsError) HandleErrorResponse(result.Errors);

            var map = _mapper.Map<List<PostInteraction>>(result.PayLoad);
            return Ok(map);
        }

        [HttpPost]
        [Route(ApiRoutes.Posts.AddInteraction)]
        [ValidateGuid("postId")]
        [ValidateModel]
        public async Task<IActionResult> AddPostInteraction(string postId, PostInteractionCreate interaction,
            CancellationToken token)
        {
            var userProfileId = HttpContext.GetUserProfileClaimValue();
            var postGuid = Guid.Parse(postId);

            var command = new AddInteraction
            {
               PostId = postGuid,
               UserProfileId = userProfileId,
               Type = interaction.Type
            };

            var result = await _mediator.Send(command, token);
            if (result.IsError) HandleErrorResponse(result.Errors);
            var map = _mapper.Map<PostInteraction>(result.PayLoad);
            return Ok(map);
        }


    }
}
