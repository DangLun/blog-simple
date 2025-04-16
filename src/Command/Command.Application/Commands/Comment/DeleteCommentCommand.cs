using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Comment
{
    public class DeleteCommentCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
