using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Notification.Commons;
using Query.Application.DTOs.Notification.OutputDTOs;
using Query.Application.Query.Notification;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Notification
{
    public class GetAllNotificationByUserIdQueryValidator : AbstractValidator<GetAllNotificationByUserIdQuery>
    {
        public GetAllNotificationByUserIdQueryValidator() {
            RuleFor(x => x.RecipientUserId).NotNull().GreaterThan(0);
        }
    }
    public class GetAllNotificationByUserIdQueryHandler : IRequestHandler<GetAllNotificationByUserIdQuery, Result<GetAllNotificationByUserIdResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllNotificationByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllNotificationByUserIdResponseDTO>> Handle(GetAllNotificationByUserIdQuery request, CancellationToken cancellationToken)
        {
            var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();
            request.PaginationOptions.SortBy ??= "NotificationAt";
            var notificationByUserIds = notificationRepo.FindAll(false, x => x.RecipientUserId == request.RecipientUserId, 
                x => x.User, x => x.Comment);

            if (request.Type != null)
            {
                notificationByUserIds = request.Type switch
                {
                    "comment" => notificationByUserIds.Where(x => x.Type.ToLower().Equals("comment")),
                    "follow" => notificationByUserIds.Where(x => x.Type.ToLower().Equals("follow")),
                    "all" => notificationByUserIds,
                };
            }

            if(request.StatusRead != null)
            {
                notificationByUserIds = request.StatusRead switch
                {
                    "unread" => notificationByUserIds.Where(x => !x.Seen),
                    "readed" => notificationByUserIds.Where(x => x.Seen),
                    "all" => notificationByUserIds,
                };
            }

            var responseQuery = notificationByUserIds.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending)
                .Select(x => new NotificationDTO
                {
                    Comment = x.Comment != null ? new CommentDTO
                    {
                        CommentText = x.Comment.CommentText,
                        CreatedAt = x.Comment.CreatedAt,
                        Id = x.Comment.Id,
                        IsDeleted = x.Comment.IsDeleted,
                        ParentCommentId = x.Comment.ParentCommentId,
                        UpdatedAt = x.Comment.UpdatedAt,
                        User = new UserDTO
                        {
                            Avatar = x.Comment.User.Avatar,
                            CreatedAt = x.Comment.User.CreatedAt,
                            Email = x.Comment.User.Email,
                            FullName = x.Comment.User.FullName,
                            Id = x.Comment.User.Id,
                            IsLoginWithGoogle= x.Comment.User.IsLoginWithGoogle   
                        }
                    } : null,
                    FolloweredMe = x.User != null ? new UserDTO
                    {
                        Avatar = x.User.Avatar,
                        IsLoginWithGoogle = x.User.IsLoginWithGoogle,
                        Id = x.User.Id,
                        FullName= x.User.FullName,
                        Email = x.User.Email,
                        CreatedAt = x.User.CreatedAt
                    } : null,
                    Seen = x.Seen,
                    Type = x.Type,
                    Id = x.Id,
                    NotificationAt = x.NotificationAt,
                    PostId = x.PostId
                });

            var response = await PagedList<NotificationDTO>.CreateAsync(responseQuery, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllNotificationByUserIdResponseDTO
            {
                Notification = response,
            });
        }
    }
}
