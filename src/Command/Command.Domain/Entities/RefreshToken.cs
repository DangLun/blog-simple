using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class RefreshToken : AggregateRoot<int>
    {
        public string Token { get; set; }
        public int UserId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsRevoked { get; set; }
        public User User { get; set; }
    }
}
