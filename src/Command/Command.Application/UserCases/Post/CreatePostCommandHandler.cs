using Command.Application.Abstractions;
using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Constants;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator() {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.ContentText).NotNull();
            RuleFor(x => x.IsPublic).NotNull();
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        public CreatePostCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }
        public async Task<Result> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            bool isPublic = request.IsPublic ?? false;

            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
                var postTextRepo = unitOfWork.Repository<PostText, int>();
                var followRepo = unitOfWork.Repository<Follow, int>();
                var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();  

                var postText = new PostText
                {
                    Content = request.ContentText
                };

                postTextRepo.Add(postText);
                await postTextRepo.SaveChangesAsync(cancellationToken);
           

                var post = new Domain.Entities.Post
                {
                    PostTitle = request.Title,
                    UserId = (int)request.UserId,
                    IsDeleted = false,
                    CreatedAt = DateTime.Now,
                    IsPublished = isPublic,
                    PostSummary = request.Summary,
                    PostTextId = postText.Id,
                    PostThumbnail = request.BannerFile != null ? await fileService.UploadFile(request.BannerFile, Const.UPLOAD_DIRECTORY) : null,
                    TotalComments = 0,
                    TotalReactions = 0,
                    TotalReads = 0,
                };

                postRepo.Add(post);
                await postRepo.SaveChangesAsync(cancellationToken);

                if (request.SelectedIds is not null && request.SelectedIds.Any())
                {
                    post.PostTags = new List<PostTag>();
                    foreach (var tagId in request.SelectedIds)
                    {
                        var postTag = new PostTag
                        {
                            PostId = post.Id,
                            TagId = tagId,
                        };
                        post.PostTags.Add(postTag);
                    }
                    await postRepo.SaveChangesAsync(cancellationToken);
                }

                // notification
                // get all RecipientUserId
                var recipientUserIds = followRepo.FindAll(false, x => x.FollowedId == request.UserId)
                    .Select(x => x.FollowerId);

                foreach(var recipientId in recipientUserIds)
                {
                    var notification = new Domain.Entities.Notification
                    {
                        Type = "Follow",
                        NotificationAt = DateTime.Now,
                        Seen = false,
                        UserId = request.UserId,
                        RecipientUserId = recipientId,
                    };

                    notificationRepo.Add(notification);
                }

                await notificationRepo.SaveChangesAsync(cancellationToken);

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
