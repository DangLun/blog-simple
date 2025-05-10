using Asp.Versioning;
using Contract.Enumerations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Query.Application.DTOs.Comment.InputDTOs;
using Query.Application.DTOs.Post.InputDTO;
using Query.Application.DTOs.User.InputDTO;
using Query.Application.Query.Comment;
using Query.Application.Query.Post;
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

        [MapToApiVersion(1)]
        [HttpGet("get-all-followed-by-user-id")]
        public async Task<IActionResult> GetAllFollowedByUserIdV1([FromQuery] GetAllFollowedByUserIdRequestDTO request)
        {
            var query = new GetAllFollowedByUserIdQuery
            {
                UserId = request.UserId,
                UserIdCall = request.UserIdCall,
                FilterOptions = new Contract.Options.FilterOptions
                {
                    IncludeActived = true,
                    IncludeDeleted = request.IncludeDeleted ?? false,
                },
                PaginationOptions = new Contract.Options.PaginationOptions(request.SortBy, request.IsDescending, request.Page, request.PageSize)
            };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }

            return BadRequest(result.Error);
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all")]
        [Authorize(Roles = nameof(PermissionType.ADMIN))]
        public async Task<IActionResult> GetAllV1([FromQuery] GetAllUserRequestDTO request)
        {
            var query = new GetAllUserQuery
            {
                SearchText = request.SearchText ?? string.Empty,
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending ?? true
                },
                StatusActiveds = request.StatusActiveds != null ?
                    JsonConvert.DeserializeObject<List<bool>>(request.StatusActiveds.ToString()) : null,
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
