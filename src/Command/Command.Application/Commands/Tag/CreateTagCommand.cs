using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class CreateTagCommand : IRequest<Result>
    {
        public string? TagName { get; set; }
        public string? ClassName { get; set; }
        public string? Description { get; set; }
    }
}
