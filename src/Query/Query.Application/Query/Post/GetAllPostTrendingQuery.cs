using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Post.OutputDTO;

namespace Query.Application.Query.Post
{
    public class GetAllPostTrendingQuery : IRequest<Result<GetAllPostTrendingResponseDTO>>
    {
        public FilterOptions? FilterOptions { get; set; }
        public bool IsRelationTag { get; set; }
        public SkipTakeOptions? SkipTakeOptions { get; set; }
        public int? UserIdCall { get; set; }
    }
}
