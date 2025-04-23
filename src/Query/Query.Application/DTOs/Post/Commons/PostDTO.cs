namespace Query.Application.DTOs.Post.Commons
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string? PostThumbnail { get; set; }
        public string? PostSummary { get; set; }
        public int PostTextId { get; set; }
        public int TotalReactions { get; set; }
        public int TotalComments { get; set; }
        public int TotalReads { get; set; }
        public bool IsPublished { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsMine { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool? IsSaved { get; set; }
        public UserDTO? Author { get; set; }
        public List<TagDTO>? Tags { get; set; }
    }
}
