using Query.Domain.Abstractions.Aggregates;

namespace Query.Domain.Entities
{
    public class Reaction : AggregateRoot<int>
    {
        public string ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public string ReactionIcon { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public ICollection<PostReaction>? PostReactions { get; set; }
    }
}
