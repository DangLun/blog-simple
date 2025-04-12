namespace Query.Application.DTOs.Post.InputDTO
{
    public class GetAllPostRequestDTO
    {
        public string? SearchText { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public bool? IncludeDeleted { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsRelationTag { get; set; }
        public int? UserIdCall { get; set; }
        public string? FollowOrRecent { get; set; }
    }
}
