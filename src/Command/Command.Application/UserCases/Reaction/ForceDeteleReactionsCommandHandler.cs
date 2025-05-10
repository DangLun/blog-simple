using Command.Application.Commands.Reaction;
using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class ForceDeteleReactionsCommandValidator : AbstractValidator<ForceDeleteReactionsCommand>
    {
        public ForceDeteleReactionsCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
        }
    }
    public class ForceDeteleReactionsCommandHandler : IRequestHandler<ForceDeleteReactionsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeteleReactionsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeleteReactionsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();
                var postReactionRepo = unitOfWork.Repository<Domain.Entities.PostReaction, int>();

                foreach (var id in request.Ids)
                {
                    var reaction = await reactionRepo.FindByIdAsync(id, true, cancellationToken);
                    if (reaction == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Reaction>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    var postReactionByReactionId = postReactionRepo.FindAll(false, x => x.ReactionId == id);
                    if (postReactionByReactionId is not null)
                    {
                        postReactionRepo.RemoveMultiple(postReactionByReactionId.ToList());
                    }
                    reactionRepo.Remove(reaction);
                }

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
