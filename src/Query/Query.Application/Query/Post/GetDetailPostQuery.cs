using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Post.OutputDTO;

namespace Query.Application.Query.Post
{
    public class GetDetailPostQuery : IRequest<Result<GetDetailPostResponseDTO>>
    {
        public int? PostId {  get; set; }
        public int? UserIdCall { get; set; }
    }
}
