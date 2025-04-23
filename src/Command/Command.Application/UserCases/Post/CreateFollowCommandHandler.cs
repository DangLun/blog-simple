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
    public class CreateFollowCommandValidator : AbstractValidator<CreateFollowCommand>
    {
        public CreateFollowCommandValidator() {
            RuleFor(x => x.UserIdCall).NotNull().GreaterThan(0);
            RuleFor(x => x.FollowedId).NotNull().GreaterThan(0);
        }
    }
    public class CreateFollowCommandHandler : IRequestHandler<CreateFollowCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreateFollowCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateFollowCommand request, CancellationToken cancellationToken)
        {
            using IDbTransaction transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var followRepo = unitOfWork.Repository<Follow, int>();
                var notificationRepo = unitOfWork.Repository<Notification, int>();
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

                
                var user = await userRepo.FindByIdAsync((int)request.UserIdCall, true, cancellationToken);
                if (user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserIdCall);
                    return Result.Failure(Error.NotFound(message));
                }

                var userFollowed = await userRepo.FindByIdAsync((int)request.FollowedId, true, cancellationToken);
                if (userFollowed == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.FollowedId);
                    return Result.Failure(Error.NotFound(message));
                }

                var follow = await followRepo.FirstOrDefaultAsync(true, x => x.FollowerId == (int)request.UserIdCall 
                && x.FollowedId == (int)request.FollowedId, cancellationToken);

                // đã follow trước đó
                if(follow != null)
                {
                    // thực hiện xóa khỏi db
                    followRepo.Remove(follow);
                }else // mới follow lần đầu
                {
                    // Thêm mới record

                    var newFollow = new Follow
                    {
                        FollowedAt = DateTime.Now,
                        FollowedId = (int)request.FollowedId,
                        FollowerId = (int)request.UserIdCall
                    };
                    followRepo.Add(newFollow);

                    // thông báo

                    var notification = new Notification
                    {
                        NotificationAt = DateTime.Now,
                        RecipientUserId = (int)request.FollowedId,
                        Seen = false,
                        Type = "follow",
                        UserId = (int)request.UserIdCall,
                    };

                    notificationRepo.Add(notification);
                    await notificationRepo.SaveChangesAsync(cancellationToken);
                }

                await followRepo.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return Result.Success();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
