using Contract.Shared;
using Query.Application.DTOs.User.Commons;

namespace Query.Application.DTOs.User.OutputDTO
{
    public class GetAllUserResponseDTO
    {
        public PagedList<UserGetAllDTO> User { get; set; }
    }
}
