using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class Role : AggregateRoot<int>
    {
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
