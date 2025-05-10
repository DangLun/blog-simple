using Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Command.Application.Commands.Reaction
{
    public class CreateReactionCommand : IRequest<Result>
    {
        public string? ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public IFormFile? ReactionIcon { get; set; }
    }
}
