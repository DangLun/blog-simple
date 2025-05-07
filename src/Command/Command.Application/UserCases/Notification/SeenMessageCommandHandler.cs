using Command.Application.Commands.Notification;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Notification
{
    public class SeenMessageCommandValidator : AbstractValidator<SeenMessageCommand> { 
        public SeenMessageCommandValidator() {
            RuleFor(x => x.NotificationId).NotNull().GreaterThan(0);
        }
    }
    public class SeenMessageCommandHandler : IRequestHandler<SeenMessageCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public SeenMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(SeenMessageCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var notificationRepo = unitOfWork.Repository<Domain.Entities.Notification, int>();
                var notificationById = await notificationRepo.FindByIdAsync((int)request.NotificationId, true, cancellationToken);

                if(notificationById == null) { 
                    var message = MessageConstant.NotFound<Domain.Entities.Notification>(x => x.Id, request.NotificationId);
                    return Result.Failure(Error.NotFound(message));
                }

                if (notificationById.Seen) return Result.Success();

                notificationById.Seen = true;
                notificationRepo.Update(notificationById);
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
