namespace Query.Application.DTOs.User.Commons
{
    public class UserGetAllDTO : UserDTO
    {
        public int? ArticlePublishedCount { get; set; }
        public int? FollowersCount { get; set; }
    }
}
