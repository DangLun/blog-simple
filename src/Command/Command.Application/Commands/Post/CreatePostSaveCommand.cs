using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Post
{
    public class CreatePostSaveCommand : IRequest<Result>
    {
        public int? PostId { get; set; }
        public int? UserIdCall { get; set; }
        public bool? Status { get; set; }
    }
}
