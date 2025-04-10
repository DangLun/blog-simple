using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class Comment : AggregateRoot<int>
    {
        public string CommentText {  get; set; }
        public int ParentCommentId {  get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int PostId {  get; set; }
        public int UserId {  get; set; }
        public Post? Post { get; set; }
        public User? User { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
