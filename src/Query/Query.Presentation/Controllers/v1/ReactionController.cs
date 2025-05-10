using Asp.Versioning;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Application.DTOs.Reaction.InputDTOs;
using Query.Application.Query.Reaction;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/reaction")]
    public class ReactionController : ApiController
    {
        private readonly IMediator mediator;

        public ReactionController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> GetAllReactionV1([FromQuery] GetAllReactionDTO request)
        {
            var query = new GetAllReactionQuery
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
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> GetDetailReactionV1([FromQuery] GetDetailReactionDTO request)
        {
            var query = new GetDetailReactionQuery
            {
                Id = request.Id,
                IsDeleted = request.IsDeleted,
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
