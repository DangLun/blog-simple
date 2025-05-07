using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class ForceDeletePostsCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; }
    }
}
