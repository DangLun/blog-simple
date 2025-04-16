namespace Query.Application.DTOs.Post.Commons
{
    public class ReactionDTO
    {
        public int Id { get; set; }
        public string ReactionName { get; set; }
        public string ReactionIcon { get; set; }
        public int Count { get; set; }
        public bool IsSelected { get; set; }
    }
}
