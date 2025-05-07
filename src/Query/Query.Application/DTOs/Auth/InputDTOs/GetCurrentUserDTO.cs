namespace Query.Application.DTOs.Auth.InputDTOs
{
    public class GetCurrentUserDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Bio { get; set; }
        public string? Avatar { get; set; }
        public string? RoleName { get; set; }
        public bool IsLoginGoogle { get; set; }
        public bool HasNewNotification { get; set; }
    }
}
