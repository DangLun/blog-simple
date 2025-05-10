namespace Query.Application.DTOs.Reaction.InputDTOs
{
    public class GetAllReactionDTO
    {
        public string? SearchText { get; set; }
        public int? Page {  get; set; }
        public int? PageSize { get; set; }
        public bool? IsDescending { get; set; }
        public string? SortBy { get; set; }
        public string? StatusDeleteds { get; set; }
    }
}
