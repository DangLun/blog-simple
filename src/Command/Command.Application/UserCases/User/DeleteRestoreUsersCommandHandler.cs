using Command.Application.Commands.User;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.User
{
    public class DeleteRestoreUsersCommandValidator : AbstractValidator<DeleteRestoreUsersCommand>
    {
        public DeleteRestoreUsersCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestoreUsersCommandHandler : IRequestHandler<DeleteRestoreUsersCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestoreUsersCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestoreUsersCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

                foreach(var id in request.Ids)
                {
                    var user = await userRepo.FindByIdAsync(id, true, cancellationToken);
                    if (user == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    user.IsActived = (bool)request.NewStatus;
                    userRepo.Update(user);
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
