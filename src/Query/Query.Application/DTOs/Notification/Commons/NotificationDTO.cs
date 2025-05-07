namespace Query.Application.DTOs.Notification.Commons
{
    public class NotificationDTO
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public bool Seen { get; set; }
        public int? PostId { get; set; }
        public DateTime NotificationAt { get; set; }
        public CommentDTO? Comment { get; set; }
        public UserDTO? FolloweredMe { get; set; }
    }
}
