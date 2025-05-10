using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.User
{
    public class DeleteRestoreUsersCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; } 
        public bool? NewStatus { get; set; } // true is delete other is restore
    }
}
