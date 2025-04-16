using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class IncreaseReadCommand : IRequest<Result>
    {
        public int? PostId { get; set; }
    }
}
