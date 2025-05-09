using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class DeleteTagCommand : IRequest<Result>
    {
        public int? Id { get; set; }
    }
}
