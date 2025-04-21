using Contract.Options;
using Contract.Shared;
using MediatR;
using Query.Application.DTOs.Search.OutputDTO;

namespace Query.Application.Query.Search
{
    public class SearchCommand : IRequest<Result<SearchResponseDTO>>
    {
        public string SearchText { get; set; }
        public PaginationOptions? PaginationOptions { get; set; }
    }
}
