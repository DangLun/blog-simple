using Command.Application.Commands.Reaction;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class DeleteTagCommandValidator : AbstractValidator<DeleteReactionCommand>
    {
        public DeleteTagCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class DeleteReactionCommandHandler : IRequestHandler<DeleteReactionCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteReactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteReactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

                var reaction = await reactionRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if (reaction == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, reaction.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                reaction.IsDeleted = true;
                reaction.UpdatedAt = DateTime.Now;
                reactionRepo.Update(reaction);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();
                return Result.Success();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
