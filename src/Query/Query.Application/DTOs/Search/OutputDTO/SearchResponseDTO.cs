using Contract.Shared;
using Query.Application.DTOs.Search.Commons;

namespace Query.Application.DTOs.Search.OutputDTO
{
    public class SearchResponseDTO
    {
        public PagedList<PostDTO>? Posts { get; set; }
        public PagedList<TagDTO>? Tags { get; set; }
        public PagedList<AuthorDTO>? Authors { get; set; }
        public int TotalItems { get; set; }
    }
}
