using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Statistical.OutputDTOs;

namespace Query.Application.Query.Statistical
{
    public class GetTotalStatisticalQuery : IRequest<Result<GetTotalStatisticalResponseDTO>>  
    {
    }
}
