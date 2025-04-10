using Query.Domain.Abstractions.Aggregates;

namespace Query.Domain.Entities
{
    public class PostText : AggregateRoot<int>
    {
        public string Content { get; set; }
        public Post? Post { get; set; } 
    }
}
