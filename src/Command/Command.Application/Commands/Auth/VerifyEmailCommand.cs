using MediatR;
using Contract.Shared;

namespace Command.Application.Command
{
    public class VerifyEmailCommand : IRequest<Result>
    {
        public string? Token { get; set; }
    }
}
