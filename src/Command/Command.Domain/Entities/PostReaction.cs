using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class PostReaction : AggregateRoot<int>
    {
        public int PostId {  get; set; }
        public int UserId { get; set; }
        public int ReactionId { get; set; }
        public bool IsActived { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Reaction? Reaction { get; set; }  
        public Post? Post { get; set; }
        public User? User { get; set; }
    }
}
