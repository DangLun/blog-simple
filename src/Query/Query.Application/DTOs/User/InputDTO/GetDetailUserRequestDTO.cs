namespace Query.Application.DTOs.User.InputDTO
{
    public class GetDetailUserRequestDTO
    {
        public int? Id { get; set; }
        public bool? IsActived { get; set; }
        public int? UserIdCall { get; set; }
    }
}
