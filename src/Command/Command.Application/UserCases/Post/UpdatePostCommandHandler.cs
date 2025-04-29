using Command.Application.Abstractions;
using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
    {
        public UpdatePostCommandValidator() {
            RuleFor(x => x.Title).NotNull();
            RuleFor(x => x.ContentText).NotNull();
            RuleFor(x => x.IsPublic).NotNull();
            RuleFor(x => x.UserIdCall).NotNull().GreaterThan(0);
        }
    }
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        public UpdatePostCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService;
        }

        public async Task<Result> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var post = await postRepo.FindByIdAsync((int)request.PostId, true, cancellationToken, x => x.PostText, x=> x.PostTags);
                var user = await userRepo.FindByIdAsync((int)request.UserIdCall, true, cancellationToken);
                if(post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.NotFound(message));
                }

                if(user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserIdCall);
                    return Result.Failure(Error.NotFound(message));
                }

                post.PostText.Content = request.ContentText;
                post.UpdatedAt = DateTime.Now;
                post.IsPublished = (bool)request.IsPublic;
                post.PostSummary = request.Summary;
                post.PostTitle = request.Title;

                if (post.PostTags != null && post.PostTags.Any())
                {
                    post.PostTags.Clear();
                }
                else post.PostTags = new List<PostTag>();

                if (request.SelectedIds != null && request.SelectedIds.Any())
                {
                    foreach(var id in request.SelectedIds)
                    {
                        post.PostTags.Add(new PostTag
                        {
                            PostId = (int)request.PostId,
                            TagId = id,
                        });
                    }
                }
                
                if(request.BannerFile != null)
                {
                    var fileName = await fileService.UploadFile(request.BannerFile, Const.UPLOAD_DIRECTORY);
                    post.PostThumbnail = fileName;
                }
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
