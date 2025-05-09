using Asp.Versioning;
using Command.Application.Commands.Post;
using Command.Application.Commands.Tag;
using Command.Presentation.Abstractions;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/tag")]
    public class TagController : ApiController
    {
        private readonly IMediator mediator;
        public TagController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("create")]
        [MapToApiVersion(1)]
        public async Task<IActionResult> CreateV1([FromBody] CreateTagCommand request)
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
        public async Task<IActionResult> UpdateV1([FromBody] UpdateTagCommand request)
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
        public async Task<IActionResult> DeleteV1([FromBody] DeleteTagCommand request)
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
        public async Task<IActionResult> DeleteRestoreTag([FromBody] DeleteRestoreTagCommand request)
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
        public async Task<IActionResult> DeleteRestoreTags([FromBody] DeleteRestoreTagsCommand request)
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
        public async Task<IActionResult> ForceDeleteTag([FromBody] ForceDeleteTagCommand request)
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
        public async Task<IActionResult> ForceDeleteTags([FromBody] ForceDeleteTagsCommand request)
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
