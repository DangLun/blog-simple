using Command.Application.Commands.Reaction;
using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class ForceDeleteReactionCommandValidator : AbstractValidator<ForceDeleteReactionCommand>
    {
        public ForceDeleteReactionCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class ForceDeleteReactionCommandHandler : IRequestHandler<ForceDeleteReactionCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeleteReactionCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeleteReactionCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();
                var postReactionRepo = unitOfWork.Repository<Domain.Entities.PostReaction, int>();

                var reaction = await reactionRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if (reaction == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Reaction>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                var postReactionByReactionId = postReactionRepo.FindAll(false, x => x.ReactionId == request.Id);
                if(postReactionByReactionId is not null)
                {
                    postReactionRepo.RemoveMultiple(postReactionByReactionId.ToList());
                }

                reactionRepo.Remove(reaction);
                await unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return Result.Success();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
