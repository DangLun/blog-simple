using Command.Domain.Abstractions.Aggregates;

namespace Command.Domain.Entities
{
    public class User : AggregateRoot<int>
    {
        public string? PasswordHash {  get; set; }
        public string FullName {  get; set; }
        public string Email {  get; set; }
        public string? Bio {  get; set; }
        public string? Avatar { get; set; }
        public bool IsDeleted { get; set; }
        public int RoleId { get; set; }
        public bool IsLoginWithGoogle { get; set; }
        public bool IsActived {  get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime? LastLoginAt { get; set; }
        public DateTime? CreatedAt {  get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Role? Role { get; set; }
        public ICollection<Post>? Posts { get; set; }
        public ICollection<EmailToken>? EmailTokens { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<PostReaction>? PostReactions { get; set; }
        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<BlackListToken>? BlackListTokens { get; set; }
        public ICollection<Follow>? Followers { get; set; } 
        public ICollection<Follow>? Followeds { get; set; }
        public ICollection<PostSaved>? PostSaveds { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
