using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class ForceDetelePostsCommandValidator : AbstractValidator<ForceDeletePostsCommand>
    {
        public ForceDetelePostsCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any(x => x > 0))
                .WithMessage("Toàn bộ Id phải > 0");
        }
    }
    public class ForceDetelePostsCommandHandler : IRequestHandler<ForceDeletePostsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDetelePostsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeletePostsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
                var postReactionsRepo = unitOfWork.Repository<Domain.Entities.PostReaction, int>();
                var savedPostsRepo = unitOfWork.Repository<Domain.Entities.PostSaved, int>();
                var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();

                foreach (var id in request.Ids)
                {
                    var post = await postRepo.FindByIdAsync(id, true, cancellationToken);
                    if (post == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    if (post.PostTags is not null) post.PostTags.Clear();

                    var commentByPostId = commentRepo.FindAll(true, x => x.PostId == id);
                    var postReactionsByPostId = postReactionsRepo.FindAll(true, x => x.PostId == id);
                    var savedPostsByPostId = savedPostsRepo.FindAll(true, x => x.PostId == id);
                    var notificationByPostId = notificationRepo.FindAll(true, x => x.PostId == id);

                    commentRepo.RemoveMultiple(commentByPostId.ToList());
                    postReactionsRepo.RemoveMultiple(postReactionsByPostId.ToList());
                    savedPostsRepo.RemoveMultiple(savedPostsByPostId.ToList());
                    notificationRepo.RemoveMultiple(notificationByPostId.ToList());
                    postRepo.Remove(post);
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
