using Command.Application.Commands.Post;
using Command.Application.Commands.Tag;
using Command.Application.Commands.User;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.User
{
    public class DeleteRestoreUserCommandValidator : AbstractValidator<DeleteRestoreUserCommand>
    {
        public DeleteRestoreUserCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestoreUserCommandHandler : IRequestHandler<DeleteRestoreUserCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestoreUserCommandHandler(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestoreUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

                var user = await userRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if(user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                user.IsActived = (bool)request.NewStatus;
                userRepo.Update(user);  
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
