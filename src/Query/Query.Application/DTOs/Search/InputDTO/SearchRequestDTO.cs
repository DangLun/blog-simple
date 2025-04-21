namespace Query.Application.DTOs.Search.InputDTO
{
    public class SearchRequestDTO
    {
        public string? SearchText { get; set; }
        public int? Page {  get; set; }
        public int? PageSize { get; set; }
        public string? SortBy {  get; set; }
        public bool? IsDescending { get; set; }
    }
}
