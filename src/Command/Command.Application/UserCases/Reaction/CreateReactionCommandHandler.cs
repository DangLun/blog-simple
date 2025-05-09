using Command.Application.Commands.Reaction;
using Command.Domain.Abstractions.Repositories;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class CreateTagCommandValidator : AbstractValidator<CreateReactionCommand>
    {
        public CreateTagCommandValidator() {
            RuleFor(x => x.ReactionName).NotNull();
            RuleFor(x => x.ReactionIcon).NotNull();
        }
    }
    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateReactionCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = _unitOfWork.Repository<Domain.Entities.Reaction, int>();

                var reaction = new Domain.Entities.Reaction
                {
                    ReactionName = request.ReactionName,
                    CreatedAt = DateTime.Now,
                    ReactionIcon = request.ReactionIcon,
                    IsDeleted = false,
                    ReactionDescription = request.ReactionDescription,
                };


                reactionRepo.Add(reaction);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                
                transaction.Commit();
                return Result.Success();
            }catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
