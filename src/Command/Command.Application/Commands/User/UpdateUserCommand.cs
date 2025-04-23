using Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Command.Application.Commands.User
{
    public class UpdateUserCommand : IRequest<Result>
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public IFormFile? Avatar { get; set; }
        public int? RoleId { get; set; }
        public bool? IsActived { get; set; }
    }
}
