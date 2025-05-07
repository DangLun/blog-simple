using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Notification.OutputDTOs;

namespace Query.Application.Query.Notification
{
    public class GetAllNotificationByUserIdQuery : IRequest<Result<GetAllNotificationByUserIdResponseDTO>>
    {
        public int? RecipientUserId { get; set; }
        public string Type { get; set; } // all, comment, follow
        public string StatusRead { get; set; } // all, unread, readed
        public PaginationOptions? PaginationOptions { get; set; }
    }
}
