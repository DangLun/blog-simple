namespace Query.Application.DTOs.Post.InputDTO
{
    public class GetAllPostSavedByUserIdRequestDTO
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public bool? IsPublish { get; set; }
        public bool? IncludeDeleted { get; set; }
        public bool? IsRelationTag { get; set; }
        public int? UserId { get; set; }
        public int? UserIdCall { get; set; }
        public bool? IsSaved { get; set; }
    }
}
