using Query.Domain.Abstractions.Aggregates;

namespace Query.Domain.Entities
{
    public class PostSaved : AggregateRoot<int>
    {
        public int UserId { get; set; }
        public int PostId {  get; set; }
        public bool IsActived { get; set; }
        public Post? Post { get; set; }
        public User? User { get; set; }
    }
}
