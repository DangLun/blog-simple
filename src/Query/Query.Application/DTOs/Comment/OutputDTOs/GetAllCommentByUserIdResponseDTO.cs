using Contract.Shared;
using Query.Application.DTOs.Comment.Commons;

namespace Query.Application.DTOs.Comment.OutputDTOs
{
    public class GetAllCommentByUserIdResponseDTO
    {
        public PagedList<CommentDTO> Comments { get; set; }
    }
}
