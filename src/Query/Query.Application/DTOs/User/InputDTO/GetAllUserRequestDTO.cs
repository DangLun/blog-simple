namespace Query.Application.DTOs.User.InputDTO
{
    public class GetAllUserRequestDTO
    {
        public string? SearchText { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public string? StatusActiveds { get; set; } // "[0,1,2]"
    }
}
