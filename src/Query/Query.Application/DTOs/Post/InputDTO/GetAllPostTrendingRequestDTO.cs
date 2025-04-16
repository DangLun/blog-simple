namespace Query.Application.DTOs.Post.InputDTO
{
    public class GetAllPostTrendingRequestDTO 
    {
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? IncludeDeleted { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsRelationTag { get; set; }
        public int? UserIdCall { get; set; }
    }
}
