using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Reaction
{
    public class DeleteReactionCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
