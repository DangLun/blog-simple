using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Notification
{
    public class SeenMessageCommand : IRequest<Result>
    {
        public int? NotificationId { get; set; }
    }
}
