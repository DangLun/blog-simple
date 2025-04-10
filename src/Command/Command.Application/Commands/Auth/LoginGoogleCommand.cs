using Command.Application.DTOs.Auth.OutputDTOs;
using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class LoginGoogleCommand : IRequest<Result<LoginResponseDTO>>
    {
        public string GoogleAccessToken { get; set; }
    }
}
