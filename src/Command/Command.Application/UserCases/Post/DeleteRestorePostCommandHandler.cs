using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class DeleteRestorePostCommandValidator : AbstractValidator<DeleteRestorePostCommand>
    {
        public DeleteRestorePostCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestorePostCommandHandler : IRequestHandler<DeleteRestorePostCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestorePostCommandHandler(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestorePostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();

                var post = await postRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if(post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                post.IsDeleted = (bool)request.NewStatus;
                postRepo.Update(post);  
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
