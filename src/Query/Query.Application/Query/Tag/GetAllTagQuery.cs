using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Tag.OutputDTOs;

namespace Query.Application.Query.Tag
{
    public class GetAllTagQuery : IRequest<Result<PagedList<GetAllTagResponseDTO>>>
    {
        public string? SearchText { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
        public FilterOptions? FilterOptions { get; set; }
        public bool? IsRelationPostTag { get; set; }
    }
}
