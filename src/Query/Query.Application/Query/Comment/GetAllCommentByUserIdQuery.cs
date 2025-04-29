using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Comment.OutputDTOs;

namespace Query.Application.Query.Comment
{
    public class GetAllCommentByUserIdQuery : IRequest<Result<GetAllCommentByUserIdResponseDTO>>
    {
        public int? UserId { get; set; }
        public int? UserIdCall { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public FilterOptions? FilterOptions { get; set; }
    }
}
