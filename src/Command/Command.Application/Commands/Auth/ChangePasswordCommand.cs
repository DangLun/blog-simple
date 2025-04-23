using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Auth
{
    public class ChangePasswordCommand : IRequest<Result>
    {
        public int? UserIdCall { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
        public string? ConfirmNewPassword { get; set; }
    }
}
