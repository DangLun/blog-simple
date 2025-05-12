using Command.Application.Commands.Notification;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Notification
{
    public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
    {
        public DeleteNotificationCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        
        public DeleteNotificationCommandHandler(IUnitOfWork unitOfWork) { 
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();

                var notification = await notificationRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if (notification == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Comment>(x => x.Id, notification.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                notificationRepo.Remove(notification);
                await unitOfWork.SaveChangesAsync(cancellationToken);

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
