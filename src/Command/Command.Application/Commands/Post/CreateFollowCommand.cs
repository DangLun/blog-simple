using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class CreateFollowCommand : IRequest<Result>
    {
        public int? UserIdCall { get; set; }
        public int? FollowedId { get; set; }
    }
}
