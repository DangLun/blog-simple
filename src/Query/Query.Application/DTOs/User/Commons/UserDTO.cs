namespace Query.Application.DTOs.User.Commons
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string? PasswordHash { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
        public RoleDTO? Role { get; set; }
        public bool IsLoginWithGoogle { get; set; }
        public bool IsActived { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
