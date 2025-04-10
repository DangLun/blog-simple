using Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Command.Application.Commands.Post
{
    public class CreatePostCommand : IRequest<Result>
    {
        public IFormFile? BannerFile { get; set; }
        public string? Title { get; set; }
        public List<int>? SelectedIds { get; set; }
        public string? Summary { get; set; }
        public string? ContentText { get; set; }
        public bool? IsPublic { get; set; }
        public int? UserId { get; set; }
    }
}
