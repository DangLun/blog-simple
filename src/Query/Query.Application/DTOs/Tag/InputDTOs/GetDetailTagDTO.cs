namespace Query.Application.DTOs.Tag.InputDTOs
{
    public class GetDetailTagDTO
    {
        public int? Id { get; set; }
        public bool? IsRelationPost { get; set; }
        public bool? IncludeNoActived { get; set; }
        public bool? IncludeDeleted { get; set; }
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
