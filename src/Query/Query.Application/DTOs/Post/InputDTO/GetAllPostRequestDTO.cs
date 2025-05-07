namespace Query.Application.DTOs.Post.InputDTO
{
    public class GetAllPostRequestDTO
    {
        public string? SearchText { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public string? StatusDeleteds { get; set; }
        public string? StatusPublisheds { get; set; }
        public bool? IsRelationTag { get; set; }
        public int? UserIdCall { get; set; }
        public string? FollowOrRecent { get; set; }
        public DateTime? CurrentDate { get; set; }
        public int? SortStatus { get; set; } // 0: tuần, 1: tháng, 2 năm, null: tất cả
    }
}
