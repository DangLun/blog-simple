using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Application.DTOs.Tag.InputDTOs;
using Query.Application.Query.Tag;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/tag")]
    public class TagController : ApiController
    {
        private readonly IMediator mediator;

        public TagController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllV1([FromQuery] GetAllTagDTO request)
        {
            var query = new GetAllTagQuery
            {
                SearchText = request.SearchText ?? string.Empty,
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending ?? true
                },
                StatusDeleteds = request.StatusDeleteds != null ?
                    JsonConvert.DeserializeObject<List<bool>>(request.StatusDeleteds.ToString()) : null,
                IsRelationPostTag = request.IsRelationPostTag ?? false,
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpGet("get-detail")]
        public async Task<IActionResult> GetDetailV1([FromQuery] GetDetailTagDTO request)
        {
            var query = new GetDetailTagQuery
            {
                Id = request.Id,
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeDeleted = request.IncludeDeleted ?? false,
                    IncludeActived = false
                },
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending ?? true
                },
                IsRelationPost = request.IsRelationPost ?? false,
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
