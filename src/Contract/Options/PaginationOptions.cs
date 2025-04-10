namespace Contract.Options
{
    public class PaginationOptions
    {
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public PaginationOptions()
        {
        }

        public PaginationOptions(string? sortBy = null, 
            bool? isDescending = null, 
            int? page = null, 
            int? pageSize = null)
        {
            SortBy = sortBy;
            IsDescending = isDescending ?? true;
            Page = Math.Max(page ?? 1, 1);
            PageSize = Math.Max(pageSize ?? 5, 1);
        }
    }
}
