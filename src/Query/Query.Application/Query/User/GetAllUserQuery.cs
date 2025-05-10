using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.User.OutputDTO;

namespace Query.Application.Query.User
{
    public class GetAllUserQuery : IRequest<Result<GetAllUserResponseDTO>>
    {
        public string? SearchText { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public List<bool>? StatusActiveds { get; set; }
    }
}
