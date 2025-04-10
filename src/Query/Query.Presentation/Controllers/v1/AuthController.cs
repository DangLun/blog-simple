using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Application.Query.Auth;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/auth")]
    public class AuthController : ApiController
    {
        private readonly IMediator mediator;

        public AuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserV1()
        {
            // bearer 
            var accessToken = Request.Headers["Authorization"].ToString().Substring(7);
            var query = new GetCurrentUserQuery { 
                AccessToken = accessToken
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
