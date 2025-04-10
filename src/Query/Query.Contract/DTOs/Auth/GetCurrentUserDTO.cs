namespace Query.Contract.DTOs.Auth
{
    public class GetCurrentUserDTO
    {
        public string FullName {  get; set; }
        public string Email { get; set; }
        public string Bio {  get; set; }
        public string Avatar { get; set; }
        public string RoleName { get; set; }
    }
}
