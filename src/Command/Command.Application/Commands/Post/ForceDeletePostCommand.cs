using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class ForceDeletePostCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
