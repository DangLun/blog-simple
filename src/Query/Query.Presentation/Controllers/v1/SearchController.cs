using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.Search.InputDTO;
using Query.Application.Query.Search;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/search")]
    public class SearchController : ApiController
    {
        private readonly IMediator mediator;

        public SearchController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet]
        public async Task<IActionResult> SeachV1([FromQuery] SearchRequestDTO request)
        {
            var query = new SearchCommand
            {
                SearchText = request.SearchText ?? string.Empty,
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending ?? true
                },
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
