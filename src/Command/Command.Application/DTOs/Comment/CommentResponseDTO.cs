using Command.Application.DTOs.Comment.Commons;

namespace Command.Application.DTOs.Comment
{
    public class CommentResponseDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PostId { get; set; }
        public int ParentCommentId { get; set; }
        public bool IsMine { get; set; }
        public AuthorCommentDTO AuthorComment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int Depth { get; set; }
    }
}
