namespace Query.Application.DTOs.Post.Commons
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int ParentCommentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public bool? IsMine { get; set; }
        public UserDTO? UserComment { get; set; }
        public int Depth { get; set; }
        public List<CommentDTO>? Replieds { get; set; }
    }
}
