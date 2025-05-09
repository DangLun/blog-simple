using Asp.Versioning;
using Command.Application.Commands.Reaction;
using Command.Presentation.Abstractions;
using MediatR;
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
        public async Task<IActionResult> CreateV1([FromBody] CreateReactionCommand request)
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
        public async Task<IActionResult> UpdateV1([FromBody] UpdateReactionCommand request)
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
    }
}
