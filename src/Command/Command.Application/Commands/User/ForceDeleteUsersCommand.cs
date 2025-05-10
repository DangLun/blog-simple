using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.User
{
    public class ForceDeleteUsersCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; }
    }
}
