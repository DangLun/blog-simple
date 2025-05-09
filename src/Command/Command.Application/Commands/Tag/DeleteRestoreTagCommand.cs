using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class DeleteRestoreTagCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public bool? NewStatus { get; set; } // true is delete other restore
    }
}
