using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.Comment.InputDTOs;
using Query.Application.Query.Auth;
using Query.Application.Query.Comment;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/comment")]
    public class CommentController : ApiController
    {
        private readonly IMediator mediator;

        public CommentController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all-by-user-id")]
        public async Task<IActionResult> GetAllByUserIdV1([FromQuery] GetAllCommentByUserIdRequestDTO request)
        {
            var query = new GetAllCommentByUserIdQuery
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
