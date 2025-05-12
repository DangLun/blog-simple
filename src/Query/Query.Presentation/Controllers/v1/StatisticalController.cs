using Asp.Versioning;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Application.Query.Statistical;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/statistical")]
    public class StatisticalController : ApiController
    {
        private readonly IMediator mediator;

        public StatisticalController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-posted-in-range-time")]
        [Authorize(Roles =nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> GetPostedInRangeTime([FromQuery] GetTotalPostedInRangeTimeQuery request)
        {
           var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpGet("get-total")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> GetTotal()
        {
            var request = new GetTotalStatisticalQuery();
            var result = await mediator.Send(request);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
    }
}
