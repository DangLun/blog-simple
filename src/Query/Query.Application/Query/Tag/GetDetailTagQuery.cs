using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Tag.OutputDTOs;

namespace Query.Application.Query.Tag
{
    public class GetDetailTagQuery : IRequest<Result<GetDetailTagResponseDTO>>
    {
        public int? Id { get; set; }
        public bool? IsRelationPost { get; set; }
        public FilterOptions? FilterOptions { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
    }
}
