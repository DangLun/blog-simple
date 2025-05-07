using Asp.Versioning;
using Command.Application.Commands.Post;
using Command.Presentation.Abstractions;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/post")]
    public class PostController : ApiController
    {
        private readonly IMediator mediator;
        public PostController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("create")]
        [MapToApiVersion(1)]
        [Authorize(Roles = $"{nameof(PermissionType.ADMIN)},{nameof(PermissionType.USER)}")]
        public async Task<IActionResult> Create([FromForm] CreatePostCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("increase-read")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> IncreaseRead([FromBody] IncreaseReadCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("update")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> UpdateV1([FromForm] UpdatePostCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("saved-post")]
        [MapToApiVersion(1)]
        [Authorize(Roles = $"{nameof(PermissionType.ADMIN)},{nameof(PermissionType.USER)}")]
        public async Task<IActionResult> SavedPost([FromBody] CreatePostSaveCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("reaction")]
        [MapToApiVersion(1)]
        [Authorize(Roles = $"{nameof(PermissionType.ADMIN)},{nameof(PermissionType.USER)}")]
        public async Task<IActionResult> CreateReactionPost([FromBody] CreateReactionCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("follow")]
        [MapToApiVersion(1)]
        [Authorize(Roles = $"{nameof(PermissionType.ADMIN)},{nameof(PermissionType.USER)}")]
        public async Task<IActionResult> Follow([FromBody] CreateFollowCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpDelete("delete-restore")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> DeleteRestorePost([FromBody] DeleteRestorePostCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpDelete("delete-restore-multiple")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> DeleteRestorePosts([FromBody] DeleteRestorePostsCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpDelete("force-delete")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> ForceDeletePost([FromBody] ForceDeletePostCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpDelete("force-delete-multiple")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> ForceDeletePosts([FromBody] ForceDeletePostsCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpPut("change-status-multiple")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> ChangeStatusMultiple([FromBody] ChangeStatusPostsCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
    }
}
