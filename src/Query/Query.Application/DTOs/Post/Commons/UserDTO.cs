namespace Query.Application.DTOs.Post.Commons
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Avatar { get; set; }
        public bool IsLoginWithGoogle { get; set; }
    }
}
