using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class Post : AggregateRoot<int>
    {
        public string PostTitle {  get; set; }
        public string? PostThumbnail {  get; set; }
        public string? PostSummary { get; set; }
        public int PostTextId {  get; set; }
        public int TotalReactions {  get; set; }
        public int TotalComments {  get; set; }
        public int TotalReads {  get; set; }
        public bool IsPublished {  get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt {  get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public PostText? PostText { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostTag>? PostTags { get; set; }
        public ICollection<PostReaction>? PostReactions { get; set;}
        public ICollection<PostSaved>? SavedByUsers { get; set; }
        public ICollection<Notification>? Notifications { get; set; }

    }
}
