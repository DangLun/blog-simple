using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class Notification : AggregateRoot<int>
    {
        public string Type { get; set; } // reaction, comment, follow, replay
        public int? CommentId { get; set; } 
        public int? ReplayForCommentId { get; set; }
        public int RecipientUserId {  get; set; } // người nhận thông báo
        public int? UserId { get; set; } // người gửi thông báo: commnet, replay, reaction
        public int? PostId {  get; set; }
        public bool Seen {  get; set; }
        public DateTime NotificationAt { get; set; }
        public Comment? Comment { get; set; }
        public Comment? ReplayForComment { get; set; }
        public User? RecipientUser { get; set; }
        public User? User { get; set; }
        public Post? Post { get; set; }
    }
}
