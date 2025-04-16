using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Comment
{
    public class UpdateCommentCommand : IRequest<Result>
    {
        public string? CommentText { get; set; }
        public int? CommentId { get; set; }
    }
}
