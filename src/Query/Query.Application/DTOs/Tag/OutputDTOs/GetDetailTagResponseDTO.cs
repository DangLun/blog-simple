using Contract.Shared;
using Query.Application.DTOs.Tag.Commons;

namespace Query.Application.DTOs.Tag.OutputDTOs
{
    public class GetDetailTagResponseDTO
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string? ClassName { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public PagedList<PostDTO>? Posts { get; set; }
    }
}
