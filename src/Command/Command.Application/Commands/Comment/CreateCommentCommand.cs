using Command.Application.DTOs.Comment;
using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Comment
{
    public class CreateCommentCommand : IRequest<Result<CommentResponseDTO>>
    {
        public string? Content { get; set; }
        public int? PostId { get; set; }
        public int? ParentCommentId { get; set; }
        public int? UserId { get; set; }
        public bool? IsReplay { get; set; }
        public int? UserIdComment { get; set; }
    }
}
