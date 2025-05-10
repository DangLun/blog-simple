using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Reaction
{
    public class ForceDeleteReactionCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
