using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class EmailToken : AggregateRoot<int>
    {
        public string Token { get; set; }
        public DateTime ExpiredAt {  get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public bool IsUsed {  get; set; }
    }
}
