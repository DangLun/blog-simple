using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Reaction.OutputDTOs;

namespace Query.Application.Query.Reaction
{
    public class GetDetailReactionQuery : IRequest<Result<GetDetailReactionResponseDTO>>
    {
        public int? Id { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
