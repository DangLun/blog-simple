using Asp.Versioning;
using Command.Application.Commands.Uploads;
using Command.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Command.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/upload")]
    public class UploadController : ApiController
    {
        private readonly IMediator mediator;
        public UploadController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("upload-file")]
        [MapToApiVersion(1)]
        [Authorize]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadCommand request)
        {
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return BadRequest(result.Error);
        }

    }
}
