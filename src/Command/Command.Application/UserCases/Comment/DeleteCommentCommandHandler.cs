using Command.Application.Commands.Comment;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Comment
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();

                var comment = await commentRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if (comment == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Comment>(x => x.Id, comment.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                comment.IsDeleted = true;
                comment.UpdatedAt = DateTime.Now;
                commentRepo.Update(comment);
                await commentRepo.SaveChangesAsync(cancellationToken);

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
