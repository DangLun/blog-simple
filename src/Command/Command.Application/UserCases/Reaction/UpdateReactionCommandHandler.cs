using Command.Application.Commands.Comment;
using Command.Application.Commands.Reaction;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateReactionCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.ReactionName).NotNull();
            RuleFor(x => x.ReactionIcon).NotNull();
        }
    }
    public class UpdateReactionCommandHandler : IRequestHandler<UpdateReactionCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateReactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateReactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();
                
                var reaction = await reactionRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if(reaction == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Comment>(x => x.Id, reaction.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                reaction.ReactionName = request.ReactionName;
                reaction.ReactionDescription = request.ReactionDescription;
                reaction.IsDeleted = request.IsDeleted ?? reaction.IsDeleted;
                reaction.ReactionIcon = request.ReactionIcon;
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
