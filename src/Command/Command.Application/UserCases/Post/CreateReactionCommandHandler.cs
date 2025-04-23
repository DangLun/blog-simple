using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using System.Data;

namespace Command.Application.UserCases.Post
{

    public class CreateReactionCommandValidator : AbstractValidator<CreateReactionCommand>
    {
        public CreateReactionCommandValidator() {
            RuleFor(x => x.PostId).NotNull().GreaterThan(0);
            RuleFor(x => x.UserIdCall).NotNull().GreaterThan(0);
            RuleFor(x => x.ReactionId).NotNull().GreaterThan(0);
        }
    }

    public class CreateReactionCommandHandler : IRequestHandler<CreateReactionCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateReactionCommandHandler(IUnitOfWork unitOfWork) { 
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateReactionCommand request, CancellationToken cancellationToken)
        {
            using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>(); 
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var postReactionRepo = unitOfWork.Repository<PostReaction, int>();
                var reactionRepo = unitOfWork.Repository<Reaction, int>();

                var post = await postRepo.FindByIdAsync((int)request.PostId, true, cancellationToken);
                if (post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.NotFound(message));
                }

                var user = await userRepo.FindByIdAsync((int)request.UserIdCall, true, cancellationToken);
                if (user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserIdCall);
                    return Result.Failure(Error.NotFound(message));
                }

                var reaction = await reactionRepo.FindByIdAsync((int)request.ReactionId, true, cancellationToken);
                if (user == null)
                {
                    var message = MessageConstant.NotFound<Reaction>(x => x.Id, request.ReactionId);
                    return Result.Failure(Error.NotFound(message));
                }

                var oldPostReaction = await postReactionRepo.FirstOrDefaultAsync(true, x => x.PostId == (int)request.PostId
                && x.UserId == (int)request.UserIdCall);

                if(oldPostReaction == null)
                {
                    var newPostReaction = new PostReaction
                    {
                        CreatedAt = DateTime.Now,
                        IsActived = true,
                        PostId = (int)request.PostId,
                        ReactionId = (int)request.ReactionId,
                        UserId = (int)request.UserIdCall
                    };
                    post.TotalReactions++;
                    postReactionRepo.Add(newPostReaction);
                }
                else
                {
                    // đổi icon tương tác
                    if(oldPostReaction.ReactionId != (int)request.ReactionId)
                    {
                        // trước đó đã bỏ tương tác một icon A và bây giờ chọn icon B mới
                        if (!oldPostReaction.IsActived)
                        {
                            post.TotalReactions++;
                            oldPostReaction.IsActived = true;
                        }
                        
                    }
                    // trước đó đã bỏ tương tác A và sau đó tương tác lại A
                    else if (oldPostReaction.ReactionId == (int)request.ReactionId && !oldPostReaction.IsActived)
                    {
                        post.TotalReactions++;
                        oldPostReaction.IsActived = true;
                    }
                    else // bỏ tương tác
                    {
                        post.TotalReactions--;
                        oldPostReaction.IsActived = false;
                    }
                    oldPostReaction.ReactionId = (int)request.ReactionId;
                    postReactionRepo.Update(oldPostReaction);
                }

                await postReactionRepo.SaveChangesAsync(cancellationToken);

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
