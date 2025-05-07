namespace Query.Application.DTOs.Notification.Commons
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public int ParentCommentId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public UserDTO User { get; set; }
    }
}
