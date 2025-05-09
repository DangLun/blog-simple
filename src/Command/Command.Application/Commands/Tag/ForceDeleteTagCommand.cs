using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class ForceDeleteTagCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
