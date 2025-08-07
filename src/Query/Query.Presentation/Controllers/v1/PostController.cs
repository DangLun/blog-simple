using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Application.DTOs.Post.InputDTO;
using Query.Application.Query.Post;
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
                    IsDescending = request.IsDescending ?? true
                },
                StatusDeleteds = request.StatusDeleteds != null ? 
                    JsonConvert.DeserializeObject<List<bool>>(request.StatusDeleteds.ToString()) : null,
                StatusPublisheds = request.StatusPublisheds != null ?
                    JsonConvert.DeserializeObject<List<bool>>(request.StatusPublisheds.ToString()) : null,
                IsRelationTag = request.IsRelationTag ?? false,
                FollowOrRecent = request.FollowOrRecent ?? "recent",
                UserIdCall = request.UserIdCall ?? null,
                CurrentDate = request.CurrentDate,
                SortStatus = request.SortStatus < 0 ? null : request.SortStatus,
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all-trending")]
        public async Task<IActionResult> GetAllTrendingV1([FromQuery] GetAllPostTrendingRequestDTO request)
        {
            var query = new GetAllPostTrendingQuery
            {
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeDeleted = request.IncludeDeleted ?? false,
                    IncludeActived = request.IsPublic ?? true
                },
                IsRelationTag = request.IsRelationTag ?? false,
                SkipTakeOptions = new Contract.Options.SkipTakeOptions(request.Skip, request.Take),
                UserIdCall = request.UserIdCall
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
        public async Task<IActionResult> GetDetailV1([FromQuery] GetDetailPostRequestDTO request)
        {
            var query = new GetDetailPostQuery
            {
                PostId = request.PostId,
                UserIdCall = request.UserIdCall
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }
        [MapToApiVersion(1)]
        [HttpGet("get-all-by-user-id")]
        public async Task<IActionResult> GetAllPostByUserIdV1([FromQuery] GetAllPostByUserIdRequestDTO request)
        {
            var query = new GetAllPostByUserIdQuery
            {
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeDeleted = request.IncludeDeleted ?? false,
                    IncludeActived = request.IsPublish ?? true
                },
                PaginationOptions = new Contract.Options.PaginationOptions(request.SortBy, request.IsDescending,
                request.Page, request.PageSize),
                IsRelationTag = request.IsRelationTag ?? false,
                UserIdCall = request.UserIdCall,
                UserId = request.UserId,
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all-saved-by-user-id")]
        public async Task<IActionResult> GetAllPostSavedByUserIdV1([FromQuery] GetAllPostSavedByUserIdRequestDTO request)
        {
            var query = new GetAllPostSavedByUserIdQuery
            {
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeDeleted = request.IncludeDeleted ?? false,
                    IncludeActived = request.IsPublish ?? true
                },
                PaginationOptions = new Contract.Options.PaginationOptions(request.SortBy, request.IsDescending,
                request.Page, request.PageSize),
                IsRelationTag = request.IsRelationTag ?? false,
                UserIdCall = request.UserIdCall,
                UserId = request.UserId,
                IsSaved = request.IsSaved ?? false
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
