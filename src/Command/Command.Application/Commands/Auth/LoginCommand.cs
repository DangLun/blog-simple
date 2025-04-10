using Command.Application.DTOs.Auth.OutputDTOs;
using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class LoginCommand : IRequest<Result<LoginResponseDTO>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
