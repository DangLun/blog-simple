using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Reaction
{
    public class CreateReactionCommand : IRequest<Result>
    {
        public string? ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public string? ReactionIcon { get; set; }
    }
}
