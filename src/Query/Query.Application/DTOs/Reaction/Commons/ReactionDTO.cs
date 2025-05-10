namespace Query.Application.DTOs.Reaction.Commons
{
    public class ReactionDTO
    {
        public int Id { get; set; }
        public string ReactionName { get; set; }
        public string? ReactionDescription { get; set; }
        public string ReactionIcon { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
