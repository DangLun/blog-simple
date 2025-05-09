using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class ChangeStatusPostsCommandValidator : AbstractValidator<ChangeStatusPostsCommand>
    {
        public ChangeStatusPostsCommandValidator() {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class ChangeStatusPostsCommandHandler : IRequestHandler<ChangeStatusPostsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ChangeStatusPostsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;   
        }
        public async Task<Result> Handle(ChangeStatusPostsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();

                foreach (var id in request.Ids)
                {
                    var post = await postRepo.FindByIdAsync(id, true, cancellationToken);
                    if (post == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    post.IsPublished = (bool)request.NewStatus;
                    postRepo.Update(post);
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
