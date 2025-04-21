using Query.Application.DTOs.User.Commons;

namespace Query.Application.DTOs.User.OutputDTO
{
    public class GetDetailUserResponseDTO
    {
        public UserDTO? User { get; set; }
        public int TotalPosted { get; set; }
        public int TotalCommented { get; set; }
        public int TotalFollower { get; set; }
        public bool IsMe { get; set; }
        public bool IsFollowed { get; set; }
    }
}
