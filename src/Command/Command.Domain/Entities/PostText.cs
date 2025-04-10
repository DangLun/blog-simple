using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class PostText : AggregateRoot<int>
    {
        public string Content { get; set; }
        public Post? Post { get; set; } 
    }
}
