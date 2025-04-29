using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.User.OutputDTO;

namespace Query.Application.Query.User
{
    public class GetAllFollowedByUserIdQuery : IRequest<Result<GetAllFollowedByUserIdResponseDTO>>
    {
        public int? UserId { get; set; }
        public int? UserIdCall { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public FilterOptions? FilterOptions { get; set; }
    }
}
