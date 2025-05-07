using Contract.Shared;
using Query.Application.DTOs.Notification.Commons;

namespace Query.Application.DTOs.Notification.OutputDTOs
{
    public class GetAllNotificationByUserIdResponseDTO
    {
        public PagedList<NotificationDTO> Notification { get; set; }
    }
}
