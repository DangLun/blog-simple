namespace Query.Application.DTOs.Comment.Commons
{
    public class CommentDTO
    {
        public string CommentText { get; set; }
        public int ParentCommentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDTO User { get; set; }
    }
}
