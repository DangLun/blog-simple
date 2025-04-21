using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Post.OutputDTO;

namespace Query.Application.Query.Post
{
    public class GetAllPostByUserIdQuery : IRequest<Result<GetAllPostByUserIdResponseDTO>>
    {
        public PaginationOptions? PaginationOptions { get; set; }
        public FilterOptions? FilterOptions { get; set; }
        public bool? IsRelationTag { get; set; }
        public int? UserId { get; set; }
        public int? UserIdCall { get; set; }
    }
}
