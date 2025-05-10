using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Reaction.Commons;
using Query.Application.DTOs.Reaction.OutputDTOs;
using Query.Application.Query.Reaction;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Reaction
{
    public class GetAllReactionQueryValidator : AbstractValidator<GetAllReactionQuery>
    {
        public GetAllReactionQueryValidator()
        {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
        }
    }
    public class GetAllReactionQueryHandler : IRequestHandler<GetAllReactionQuery, Result<GetAllReactionResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllReactionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllReactionResponseDTO>> Handle(GetAllReactionQuery request, CancellationToken cancellationToken)
        {
            var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();
            IQueryable<Domain.Entities.Reaction> query = reactionRepo.FindAll();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                query = query.Where(p =>
                    p.ReactionName.ToLower().Contains(request.SearchText.ToLower()));
            }

            // nếu không truyền deleted thì loại bỏ chỉ lấy isDeleted = false
            if (request.StatusDeleteds is not null)
                query = query.Where(x => request.StatusDeleteds.Contains(x.IsDeleted));

            query = query.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);

            var responseQuery = query.Select(x => new ReactionDTO
            {
                Id = x.Id,
                ReactionName = x.ReactionName,
                CreatedAt = x.CreatedAt,
                IsDeleted = x.IsDeleted,
                ReactionDescription = x.ReactionDescription,
                ReactionIcon = x.ReactionIcon,
                UpdatedAt = x.UpdatedAt,
            });

            var response = await PagedList<ReactionDTO>
                .CreateAsync(responseQuery, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllReactionResponseDTO
            {
                Reaction = response
            });
        }
    }
}
