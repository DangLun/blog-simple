namespace Query.Application.DTOs.Post.Commons
{
    public class TagDTO
    {
        public int Id { get; set; }
        public string TagName { get; set; }
        public string? ClassName { get; set; }
        public bool IsDeleted { get; set; }
        public string? Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
