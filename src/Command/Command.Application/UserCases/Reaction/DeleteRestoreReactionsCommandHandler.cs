using Command.Application.Commands.Reaction;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Reaction
{
    public class DeleteRestoreReactionsCommandValidator : AbstractValidator<DeleteRestoreReactionsCommand>
    {
        public DeleteRestoreReactionsCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestoreReactionsCommandHandler : IRequestHandler<DeleteRestoreReactionsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestoreReactionsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestoreReactionsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var reactionRepo = unitOfWork.Repository<Domain.Entities.Reaction, int>();
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var postReactionRepo = unitOfWork.Repository<Domain.Entities.PostReaction, int>();
                foreach (var id in request.Ids)
                {
                    var reaction = await reactionRepo.FindByIdAsync(id, true, cancellationToken);
                    if (reaction == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Reaction>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }

                    var posts = postRepo.FindAll();
                    var postReactions = postReactionRepo.FindAll();
                    var needPosts = from p in posts
                                    join pr in postReactions on p.Id equals pr.PostId
                                    where request.Ids.Contains(pr.ReactionId)
                                    group pr by p.Id into g
                                    select new
                                    {
                                        Post = g.First().Post,
                                        ReactionCount = g.Count()
                                    };

                    // nếu xóa thì giảm số lượng tương tác của các bài viết liên quan 
                    if (request.NewStatus is not null && request.NewStatus.Value)
                    {
                        foreach (var post in needPosts)
                        {
                            post.Post.TotalReactions -= post.ReactionCount;
                            postRepo.Update(post.Post);
                        }
                    }
                    else
                    {
                        foreach (var post in needPosts)
                        {
                            post.Post.TotalReactions += post.ReactionCount;
                            postRepo.Update(post.Post);
                        }
                    }

                    reaction.IsDeleted = (bool)request.NewStatus;
                    reactionRepo.Update(reaction);
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
