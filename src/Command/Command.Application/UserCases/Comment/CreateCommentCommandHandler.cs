using Command.Application.Commands.Comment;
using Command.Application.DTOs.Comment;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Comment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator() {
            RuleFor(x => x.Content).NotNull();
            RuleFor(x => x.PostId).NotNull().GreaterThan(0);
            RuleFor(x => x.ParentCommentId).NotNull().GreaterThanOrEqualTo(0);   
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result<CommentResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CommentResponseDTO>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var commentRepo = _unitOfWork.Repository<Domain.Entities.Comment, int>();
                var postRepo = _unitOfWork.Repository<Domain.Entities.Post, int>();
                var userRepo = _unitOfWork.Repository<Domain.Entities.User, int>();
                var notificationRepo = _unitOfWork.Repository<Domain.Entities.Notification, int>();

                var postExist = await postRepo.FindByIdAsync((int)request.PostId, false, cancellationToken);

                if (postExist is null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.Conflict(message));
                }

                var userExist = await userRepo.FindByIdAsync((int)request.UserId, false, cancellationToken);

                if (userExist is null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserId);
                    return Result.Failure(Error.Conflict(message));
                }

                var comment = new Domain.Entities.Comment
                {
                    PostId = (int)request.PostId,
                    UserId = (int)request.UserId,
                    CommentText = request.Content!,
                    ParentCommentId = request.ParentCommentId == null ? 0 : (int)request.ParentCommentId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                };
                commentRepo.Add(comment);
                await commentRepo.SaveChangesAsync(cancellationToken);

                var response = new CommentResponseDTO
                {
                    Id = comment.Id,
                    Content = comment.CommentText,
                    PostId = (int)request.PostId,
                    IsMine = true,
                    IsDeleted = comment.IsDeleted,
                    ParentCommentId = comment.ParentCommentId,
                    CreatedAt = (DateTime)comment.CreatedAt,
                    AuthorComment = new DTOs.Comment.Commons.AuthorCommentDTO
                    {
                        Id = userExist.Id,
                        Avatar = userExist.Avatar,
                        FullName = userExist.FullName,
                        isLoginGoogle = userExist.IsLoginWithGoogle,
                    },
                    Depth = GetDepthComment(comment.Id, comment.PostId)
                };

                // cập nhật lại post
                postExist.TotalComments++;
                postRepo.Update(postExist);
                await postRepo.SaveChangesAsync(cancellationToken);


                // notification 
                if(request.IsReplay != null && (bool)request.IsReplay && request.UserIdComment != null)
                {
                    var notification = new Domain.Entities.Notification
                    {
                        CommentId = comment.Id,
                        NotificationAt = DateTime.Now,
                        PostId = (int)request.PostId,
                        RecipientUserId = (int)request.UserIdComment,
                        ReplayForCommentId = comment.ParentCommentId,
                        Seen = false,
                        Type = "comment",
                        UserId = request.UserId
                    };
                    notificationRepo.Add(notification);

                    await notificationRepo.SaveChangesAsync(cancellationToken); 
                }
                transaction.Commit();
                return Result.Success(response);
            }catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
        private int GetDepthComment(int commentId, int postId)
        {
            var commentRepo = _unitOfWork.Repository<Domain.Entities.Comment, int>();

            int depth = 1;

            var comments = commentRepo.FindAll(false, x => x.PostId == postId).OrderBy(x => x.Id).ToList();

            var comment = comments.FirstOrDefault(x => x.Id == commentId);

            if (comment == null || comment.ParentCommentId == 0) return 1;


            while (comment.ParentCommentId != 0)
            {
                depth++;
                int l = 0, r = comments.Count() - 1;
                Domain.Entities.Comment? parentComment = null;
                while (l <= r)
                {
                    int mid = l + r >> 1;
                    if (comments[mid].Id == comment.ParentCommentId)
                    {
                        parentComment = comments[mid];
                        break;
                    }
                    else if (comments[mid].Id > comment.ParentCommentId) r = mid - 1;
                    else l = mid + 1;
                }
                if (parentComment == null) return depth;
                comment = parentComment;
            }
            return depth;
        }
    }
}
