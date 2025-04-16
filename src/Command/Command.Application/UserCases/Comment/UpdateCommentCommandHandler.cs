using Command.Application.Commands.Comment;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Comment
{
    public class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
    {
        public UpdateCommentCommandValidator()
        {
            RuleFor(x => x.CommentId).NotNull().GreaterThan(0);
            RuleFor(x => x.CommentText).NotNull();
        }
    }
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
                
                var comment = await commentRepo.FirstOrDefaultAsync(true, x => x.Id == request.CommentId, cancellationToken);

                if(comment == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Comment>(x => x.Id, comment.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                comment.CommentText = request.CommentText!;
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
