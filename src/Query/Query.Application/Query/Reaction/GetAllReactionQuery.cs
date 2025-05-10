using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Reaction.OutputDTOs;

namespace Query.Application.Query.Reaction
{
    public class GetAllReactionQuery : IRequest<Result<GetAllReactionResponseDTO>>
    {
        public string? SearchText { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public List<bool>? StatusDeleteds { get; set; }
    }
}
