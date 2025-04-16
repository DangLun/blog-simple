namespace Query.Application.DTOs.Post.Commons
{
    public class AuthorDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public bool IsLoginWithGoogle { get; set; }
        public DateTime? JoinedDate { get; set; }
        public string? Bio { get; set; }
    }
}
