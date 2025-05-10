using Asp.Versioning;
using Command.Application.Commands.Reaction;
using Command.Presentation.Abstractions;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/reaction")]
    public class ReactionController : ApiController
    {
        private IMediator mediator;
        public ReactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPost("create")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> CreateV1([FromForm] CreateReactionCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPut("update")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> UpdateV1([FromForm] UpdateReactionCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
        [HttpDelete("delete")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> DeleteV1([FromBody] DeleteReactionCommand request)
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
        public async Task<IActionResult> DeleteRestoreReaction([FromBody] DeleteRestoreReactionCommand request)
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
        public async Task<IActionResult> DeleteRestoreReactions([FromBody] DeleteRestoreReactionsCommand request)
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
        public async Task<IActionResult> ForceDeleteReaction([FromBody] ForceDeleteReactionCommand request)
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
        public async Task<IActionResult> ForceDeleteReactions([FromBody] ForceDeleteReactionsCommand request)
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
