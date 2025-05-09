using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class DeleteRestoreTagsCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; } 
        public bool? NewStatus { get; set; } // true is delete other is restore
    }
}
