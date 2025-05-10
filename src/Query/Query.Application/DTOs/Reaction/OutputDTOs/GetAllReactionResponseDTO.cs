using Contract.Shared;
using Query.Application.DTOs.Reaction.Commons;

namespace Query.Application.DTOs.Reaction.OutputDTOs
{
    public class GetAllReactionResponseDTO
    {
        public PagedList<ReactionDTO> Reaction { get; set; }
    }
}
