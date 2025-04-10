namespace Query.Application.DTOs.Tag.InputDTOs
{
    public class GetAllTagDTO
    {
        public string? SearchText { get; set; }
        public int? Page {  get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public bool? IncludeDeleted { get; set; }
        public bool? IsRelationPostTag { get; set; }
    }
}
