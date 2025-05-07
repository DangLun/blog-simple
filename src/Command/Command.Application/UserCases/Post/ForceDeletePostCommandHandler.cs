using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class ForceDeletePostCommandValidator : AbstractValidator<ForceDeletePostCommand>
    {
        public ForceDeletePostCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class ForceDeletePostCommandHandler : IRequestHandler<ForceDeletePostCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeletePostCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeletePostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
                var postReactionsRepo = unitOfWork.Repository<Domain.Entities.PostReaction, int>();
                var savedPostsRepo = unitOfWork.Repository<Domain.Entities.PostSaved, int>();
                var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();

                var post = await postRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if (post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                
                if(post.PostTags is not null) post.PostTags.Clear();

                var commentByPostId = commentRepo.FindAll(true, x => x.PostId == request.Id);
                var postReactionsByPostId = postReactionsRepo.FindAll(true, x => x.PostId == request.Id);
                var savedPostsByPostId = savedPostsRepo.FindAll(true, x => x.PostId == request.Id);
                var notificationByPostId = notificationRepo.FindAll(true, x => x.PostId == request.Id);

                commentRepo.RemoveMultiple(commentByPostId.ToList());
                postReactionsRepo.RemoveMultiple(postReactionsByPostId.ToList());
                savedPostsRepo.RemoveMultiple(savedPostsByPostId.ToList());
                notificationRepo.RemoveMultiple(notificationByPostId.ToList());

                postRepo.Remove(post);
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
