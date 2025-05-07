namespace Query.Application.DTOs.Notification.InputDTOs
{
    public class GetAllNotificationByUserIdRequestDTO
    {
        public int? RecipientUserId { get; set; }
        public string? Type { get; set; }
        public string? StatusRead { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set;}
        public string? SortBy { get; set; }
        public bool? IsDescending { get; set; }
    }
}
