using Asp.Versioning;
using Command.Application.Commands.Notification;
using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/notification")]
    public class NotificationController : ApiController
    {
        private readonly IMediator mediator;
        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpPost("seen")]
        public async Task<IActionResult> SeenMessage([FromBody] SeenMessageCommand request)
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
