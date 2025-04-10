using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Auth.InputDTOs;

namespace Query.Application.Query.Auth
{
    public class GetCurrentUserQuery : IRequest<Result<GetCurrentUserDTO>>
    {
        public string AccessToken { get; set; }
    }
}
