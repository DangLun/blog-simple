using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Statistical.Commons;
using Query.Application.DTOs.Statistical.OutputDTOs;
using Query.Application.Query.Statistical;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Statistical
{
    public class GetTotalPostedInRangeTimeQueryValidator : AbstractValidator<GetTotalPostedInRangeTimeQuery>
    {
        public GetTotalPostedInRangeTimeQueryValidator()
        {
            RuleFor(x => x.StartDate).NotNull();
            RuleFor(x => x.EndDate).NotNull();
        }
    }
    public class GetTotalPostedInRangeTimeQueryHandler : IRequestHandler<GetTotalPostedInRangeTimeQuery, 
        Result<GetPostedInRangeTimeResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetTotalPostedInRangeTimeQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetPostedInRangeTimeResponseDTO>> Handle(GetTotalPostedInRangeTimeQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();

            var posts = postRepo.FindAll(false, x => x.IsPublished && !x.IsDeleted
                && x.CreatedAt >= request.StartDate && x.CreatedAt <= request.EndDate);

            var responsePosts = posts
                .GroupBy(p => p.CreatedAt.Value.Date)
                .Select(g => new PostedInRangeTimeDTO
                {
                    Date = g.Key,   
                    PostedCount = g.Count() 
                })
                .ToList();

            return Result.Success(new GetPostedInRangeTimeResponseDTO
            {
                Posts = responsePosts
            });
        }
    }
}
