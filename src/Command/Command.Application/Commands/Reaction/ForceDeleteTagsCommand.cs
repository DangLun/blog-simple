using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Reaction
{
    public class ForceDeleteReactionsCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; }
    }
}
