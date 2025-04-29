namespace Query.Application.DTOs.Comment.InputDTOs
{
    public class GetAllCommentByUserIdRequestDTO
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public bool? IncludeDeleted { get; set; }
        public int? UserId { get; set; }
        public int? UserIdCall { get; set; }
    }
}
