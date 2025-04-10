using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class Follow : AggregateRoot<int>
    {
        public int FollowerId { get; set; }
        public int FollowedId { get; set; }
        public DateTime? FollowedAt { get; set; }
        public User? Follower {  get; set; }
        public User? Followed {  get; set; }
    }
}
