using Command.Application.DTOs.Auth.OutputDTOs;
using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class RefreshTokenCommand : IRequest<Result<RefreshTokenResponseDTO>>
    {
        public string RefreshToken { get; set; }
        public string OldAccessToken { get; set; }
    }
}
