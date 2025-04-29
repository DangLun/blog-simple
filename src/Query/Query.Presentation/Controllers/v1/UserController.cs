using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.Comment.InputDTOs;
using Query.Application.DTOs.User.InputDTO;
using Query.Application.Query.Comment;
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
    }
}
