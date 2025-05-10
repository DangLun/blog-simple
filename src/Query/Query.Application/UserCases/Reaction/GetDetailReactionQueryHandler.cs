using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Reaction.OutputDTOs;
using Query.Application.Query.Reaction;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Reaction
{
    public class GetDetailReactionQueryValidator : AbstractValidator<GetDetailReactionQuery>
    {
        public GetDetailReactionQueryValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class GetDetailReactionQueryHandler : IRequestHandler<GetDetailReactionQuery, Result<GetDetailReactionResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDetailReactionQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetDetailReactionResponseDTO>> Handle(GetDetailReactionQuery request, CancellationToken cancellationToken)
        {
            var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();

            var reaction = await reactionRepo.FirstOrDefaultAsync(false,x => x.Id == request.Id, cancellationToken);

            if (reaction is null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.Reaction>(x => x.Id, request.Id);
                return Result.Failure(Error.NotFound(message));
            }

            if(request.IsDeleted is null || !request.IsDeleted.Value)
            {
                if(reaction.IsDeleted)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Reaction>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }
            }

            var response = new GetDetailReactionResponseDTO
            {
                Reaction = new DTOs.Reaction.Commons.ReactionDTO
                {
                    Id = reaction.Id,
                    CreatedAt = reaction.CreatedAt,
                    ReactionDescription = reaction.ReactionDescription,
                    IsDeleted = reaction.IsDeleted,
                    ReactionIcon = reaction.ReactionIcon,
                    ReactionName = reaction.ReactionName,
                    UpdatedAt = reaction.UpdatedAt,
                }
            };

            return Result.Success(response);
        }
    }
}
