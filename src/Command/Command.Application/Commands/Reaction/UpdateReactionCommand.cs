using Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Command.Application.Commands.Reaction
{
    public class UpdateReactionCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public string? ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public IFormFile? ReactionIcon { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
