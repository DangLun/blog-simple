using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Post.OutputDTO;

namespace Query.Application.Query.Post
{
    public class GetAllPostQuery : IRequest<Result<GetAllPostResponseDTO>>
    {
        public string? SearchText { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public FilterOptions? FilterOptions { get; set; }
        public bool? IsRelationTag { get; set; }
        public string? FollowOrRecent { get; set; }
        public int? UserIdCall { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int? SortStatus { get; set; } // 0: tuần, 1: tháng, 2 năm, null: tất cả
    }
}
