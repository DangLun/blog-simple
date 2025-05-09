using Contract.Shared;
using MediatR;

namespace Command.Application.Commands.Tag
{
    public class UpdateTagCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public string? TagName { get; set; }
        public string? ClassName { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
