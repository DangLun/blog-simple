using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.Post.InputDTO;
using Query.Application.DTOs.Tag.InputDTOs;
using Query.Application.Query.Post;
using Query.Application.Query.Tag;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/post")]
    public class PostController : ApiController
    {
        private readonly IMediator mediator;

        public PostController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllV1([FromQuery] GetAllPostRequestDTO request)
        {
            var query = new GetAllPostQuery
            {
                SearchText = request.SearchText ?? string.Empty,
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending
                },
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeDeleted = request.IncludeDeleted ?? false,
                    IncludeActived = request.IsPublic ?? true
                },
                IsRelationTag = request.IsRelationTag ?? false,
                FollowOrRecent = request.FollowOrRecent ?? "recent",
                UserIdCall = request.UserIdCall ?? null
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
