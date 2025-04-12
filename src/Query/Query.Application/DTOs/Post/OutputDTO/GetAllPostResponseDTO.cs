using Contract.Shared;
using Query.Application.DTOs.Post.Commons;

namespace Query.Application.DTOs.Post.OutputDTO
{
    public class GetAllPostResponseDTO
    {
        public PagedList<PostDTO>? Posts { get; set; }
    }
}
