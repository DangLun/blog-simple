using Query.Domain.Abstractions.Aggregates;

namespace Query.Domain.Entities
{
    public class Role : AggregateRoot<int>
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
