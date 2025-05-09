using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Tag.Commons;
using Query.Application.DTOs.Tag.OutputDTOs;
using Query.Application.Query.Tag;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Tag
{
    public class GetAllTagQueryValidator : AbstractValidator<GetAllTagQuery>
    {
        public GetAllTagQueryValidator() {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
        }
    }

    public class GetAllTagQueryHandler : IRequestHandler<GetAllTagQuery, Result<PagedList<GetAllTagResponseDTO>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllTagQueryHandler(IUnitOfWork unitOfWork) {
            _unitOfWork = unitOfWork;   
        }
        public async Task<Result<PagedList<GetAllTagResponseDTO>>> Handle(GetAllTagQuery request, CancellationToken cancellationToken)
        {
            var tagRepo = _unitOfWork.Repository<Domain.Entities.Tag, int>();
            IQueryable<Domain.Entities.Tag> query = tagRepo.FindAll();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(p =>
                    p.TagName.Contains(request.SearchText));
            }

            // nếu không truyền deleted thì loại bỏ chỉ lấy isDeleted = false
            if (request.StatusDeleteds is not null)
                query = query.Where(x => request.StatusDeleteds.Contains(x.IsDeleted));

            query = query.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);

            var responseQuery = query.Select(x => new GetAllTagResponseDTO
            {
                Id = x.Id,
                ClassName = x.ClassName,
                CreatedAt = x.CreatedAt,
                IsDeleted = x.IsDeleted,
                Description = x.Description,
                PostTags = (request.IsRelationPostTag != null && (bool)request.IsRelationPostTag) 
                ? x.PostTags.Select(x => new PostTagDTO
                {
                    PostId = x.PostId,
                    TagId = x.TagId
                }).ToList() : null,
                TagName = x.TagName,
                UpdatedAt = x.UpdatedAt,
            });

            var response = await PagedList<GetAllTagResponseDTO>
                .CreateAsync(responseQuery, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(response);
        }
    }
}
