using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class ForceDeleteTagsCommand : IRequest<Result>
    {
        public List<int>? Ids { get; set; }
    }
}
