using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class BlackListToken : AggregateRoot<int>
    {
        public string TokenRevoked { get; set; }
        public string? Reason {  get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; }
    }
}
