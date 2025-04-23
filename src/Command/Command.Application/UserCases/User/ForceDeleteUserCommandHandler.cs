using Command.Application.Commands.User;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.User
{
    public class ForceDeleteUserCommandValidator : AbstractValidator<ForceDeleteUserCommand>
    {
        public ForceDeleteUserCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class ForceDeleteUserCommandHandler : IRequestHandler<ForceDeleteUserCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeleteUserCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(ForceDeleteUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var emailTokenRepo = unitOfWork.Repository<EmailToken, int>();
                var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
                var postReactionRepo = unitOfWork.Repository<PostReaction, int>();
                var blackListRepo = unitOfWork.Repository<BlackListToken, int>();
                var followRepo = unitOfWork.Repository<Follow, int>();
                var refreshTokenRepo = unitOfWork.Repository<RefreshToken, int>();
                var postSavedRepo = unitOfWork.Repository<PostSaved, int>();
                var notificationRepo = unitOfWork.Repository<Notification, int>();
                var postTextRepo = unitOfWork.Repository<PostText, int>();

                var user = await userRepo.FindByIdAsync((int)request.Id, true, cancellationToken);

                if(user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                var postsByUserId = postRepo.FindAll(true, x => x.UserId == (int)request.Id, x => x.PostTags);
                var commentsByUserId = commentRepo.FindAll(true, x => x.UserId == (int)request.Id
                    || postsByUserId.Select(y => y.Id).ToList().Contains(x.PostId));
                var emailTokensByUserId = emailTokenRepo.FindAll(true, x => x.UserId == (int)request.Id);
                var postReactionsByUserId = postReactionRepo.FindAll(true, x => x.UserId == (int)request.Id
                    || postsByUserId.Select(y => y.Id).ToList().Contains(x.PostId));
                var blackListTokensByUserId = blackListRepo.FindAll(true, x => x.UserId == (int)request.Id);
                var followsByUserId = followRepo.FindAll(true, x => x.FollowerId == (int)request.Id || x.FollowedId == (int)request.Id);
                var refreshTokensByUserId = refreshTokenRepo.FindAll(true, x => x.UserId == (int)request.Id);
                var postSavedsByUserId = postSavedRepo.FindAll(true, x => x.UserId == (int)request.Id 
                    || postsByUserId.Select(y => y.Id).ToList().Contains(x.PostId));
                var notificationsByUserId = notificationRepo.FindAll(true, x => x.UserId == (int)request.Id 
                    || commentsByUserId.Select(y => y.Id).ToList().Contains((int)x.CommentId) || x.RecipientUserId == (int)request.Id);
                var postTextsByPosts = postTextRepo.FindAll(true, x => postsByUserId.Select(y => y.PostTextId).ToList().Contains(x.Id));
                // delete all 
                blackListRepo.RemoveMultiple(blackListTokensByUserId.ToList());
                emailTokenRepo.RemoveMultiple(emailTokensByUserId.ToList());
                refreshTokenRepo.RemoveMultiple(refreshTokensByUserId.ToList());
                postReactionRepo.RemoveMultiple(postReactionsByUserId.ToList());
                postTextRepo.RemoveMultiple(postTextsByPosts.ToList());
                foreach(var post in postsByUserId)
                {
                    if(post.PostTags != null) post.PostTags = null;
                }
                followRepo.RemoveMultiple(followsByUserId.ToList());
                commentRepo.RemoveMultiple(commentsByUserId.ToList());
                postSavedRepo.RemoveMultiple(postSavedsByUserId.ToList());
                notificationRepo.RemoveMultiple(notificationsByUserId.ToList());
                postRepo.RemoveMultiple(postsByUserId.ToList());
                userRepo.Remove(user);
                await unitOfWork.SaveChangesAsync(cancellationToken);

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
