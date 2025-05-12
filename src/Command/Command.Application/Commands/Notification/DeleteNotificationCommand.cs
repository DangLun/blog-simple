using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Notification
{
    public class DeleteNotificationCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
