using Contract.Shared;
using MediatR;
using Query.Application.DTOs.User.OutputDTO;

namespace Query.Application.Query.User
{
    public class GetDetailUserQuery : IRequest<Result<GetDetailUserResponseDTO>>
    {
        public int? Id { get; set; }
        public bool? IsActived { get; set;}
        public int? UserIdCall { get; set; }
    }
}
