using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{

    public class IncreaseReadCommandValidator : AbstractValidator<IncreaseReadCommand>
    {
        public IncreaseReadCommandValidator() {
            RuleFor(x => x.PostId).NotNull();
        }
    }

    public class IncreaseReadCommandHandler : IRequestHandler<IncreaseReadCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public IncreaseReadCommandHandler(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(IncreaseReadCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var post = await postRepo.FindByIdAsync((int)request.PostId, true, cancellationToken);

                if(post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.NotFound(message));
                }

                post.TotalReads++;
                postRepo.Update(post);
                await postRepo.SaveChangesAsync(cancellationToken);

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
