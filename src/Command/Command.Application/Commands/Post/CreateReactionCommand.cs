using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class CreateReactionCommand : IRequest<Result>
    {
        public int? PostId { get; set; }
        public int? UserIdCall { get; set; }
        public int? ReactionId { get; set; }
    }
}
