using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.User
{
    public class ForceDeleteUserCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
