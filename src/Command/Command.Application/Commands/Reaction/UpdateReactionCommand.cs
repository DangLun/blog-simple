using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Reaction
{
    public class UpdateReactionCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public string? ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public string? ReactionIcon { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
