using Asp.Versioning;
using Command.Application.Commands.Comment;
using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{

    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/comment")]
    public class CommentController : ApiController
    {
        private IMediator _mediator;
        public CommentController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("create")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCommentCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("update")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] UpdateCommentCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }

        [HttpPost("delete")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody] DeleteCommentCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result.Error);
        }
    }
}
