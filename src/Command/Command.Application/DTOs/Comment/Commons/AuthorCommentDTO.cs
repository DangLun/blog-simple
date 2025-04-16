namespace Command.Application.DTOs.Comment.Commons
{
    public class AuthorCommentDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public bool isLoginGoogle { get; set; }
    }
}
