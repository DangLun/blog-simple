using Contract.Shared;
using Query.Application.DTOs.User.Commons;

namespace Query.Application.DTOs.User.OutputDTO
{
    public class GetAllFollowedByUserIdResponseDTO
    {
        public PagedList<UserDTO> User { get; set; }
    }
}
