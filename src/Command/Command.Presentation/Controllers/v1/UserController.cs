using Asp.Versioning;
using Command.Application.Commands.User;
using Command.Presentation.Abstractions;
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
    }
}
