using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Comment
{
    public class CreateCommentCommand : IRequest<Result>
    {
        public string? Content { get; set; }
        public int? PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public int? UserId { get; set; }
    }
}
