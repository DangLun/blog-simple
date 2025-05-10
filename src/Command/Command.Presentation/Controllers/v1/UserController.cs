using Asp.Versioning;
using Command.Application.Commands.Tag;
using Command.Application.Commands.User;
using Command.Presentation.Abstractions;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/user")]
    public class UserController : ApiController
    {
        private readonly IMediator mediator;
        public UserController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [HttpPut("update")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> UpdateV1([FromForm] UpdateUserCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
        [HttpDelete("force-delete")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> ForceDeleteV1([FromBody] ForceDeleteUserCommand request)
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
        public async Task<IActionResult> DeleteRestoreUser([FromBody] DeleteRestoreUserCommand request)
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
        public async Task<IActionResult> DeleteRestoreUsers([FromBody] DeleteRestoreUsersCommand request)
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
        public async Task<IActionResult> ForceDeleteUsers([FromBody] ForceDeleteUsersCommand request)
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
