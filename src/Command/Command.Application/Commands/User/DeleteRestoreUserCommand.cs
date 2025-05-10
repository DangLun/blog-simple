using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.User
{
    public class DeleteRestoreUserCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public bool? NewStatus { get; set; } // true is delete other restore
    }
}
