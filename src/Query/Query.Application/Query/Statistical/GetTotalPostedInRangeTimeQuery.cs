using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Statistical.OutputDTOs;

namespace Query.Application.Query.Statistical
{
    public class GetTotalPostedInRangeTimeQuery : IRequest<Result<GetPostedInRangeTimeResponseDTO>>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
