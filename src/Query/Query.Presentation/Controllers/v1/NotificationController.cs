using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Query.Application.DTOs.Notification.InputDTOs;
using Query.Application.Query.Notification;
using Query.Presentation.Abstractions;

namespace Query.Presentation.Controllers.v1
{
    [ApiVersion(1)]
    [Route("api/v{v:apiVersion}/notification")]
    public class NotificationController : ApiController
    {
        private readonly IMediator mediator;

        public NotificationController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [MapToApiVersion(1)]
        [HttpGet("get-all-by-user-id")]
        public async Task<IActionResult> GetAllByUserIdV1([FromQuery] GetAllNotificationByUserIdRequestDTO request)
        {
            var query = new GetAllNotificationByUserIdQuery
            {
                PaginationOptions = new Contract.Options.PaginationOptions
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    SortBy = request.SortBy,
                    IsDescending = request.IsDescending ?? true
                },
                Type = request.Type ?? "all",
                RecipientUserId = request.RecipientUserId,
                StatusRead = request.StatusRead ?? "all"
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
