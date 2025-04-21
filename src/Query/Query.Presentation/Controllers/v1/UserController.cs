using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.User.InputDTO;
using Query.Application.Query.User;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
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

        [MapToApiVersion(1)]
        [HttpGet("get-detail")]
        public async Task<IActionResult> GetDetailUserV1([FromQuery] GetDetailUserRequestDTO request)
        {
            var query = new GetDetailUserQuery
            {
                Id = request.Id,
                IsActived = request.IsActived != null ? request.IsActived : true,
                UserIdCall = request.UserIdCall
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}
