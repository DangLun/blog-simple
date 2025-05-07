using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class ChangeStatusPostsCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; }
        public bool? NewStatus { get; set; } // true is published other draft
    }
}
