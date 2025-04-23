using Command.Application.Commands.Auth;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator() {
            RuleFor(x => x.UserIdCall).NotNull().GreaterThan(0);
            RuleFor(x => x.OldPassword).NotNull();
            RuleFor(x => x.NewPassword).NotNull();
            RuleFor(x => x.ConfirmNewPassword).NotNull();
        }
    }
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ChangePasswordCommandHandler(IUnitOfWork unitOfWork) { 
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var user = await userRepo.FindByIdAsync((int)request.UserIdCall, true, cancellationToken);

                if(user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserIdCall);
                    return Result.Failure(Error.NotFound(message));
                }

                if (user.IsLoginWithGoogle)
                {
                    return Result.Failure(Error.Conflict("Bạn không thể dùng chức năng này!"));
                }

                if (request.NewPassword != request.ConfirmNewPassword)
                {
                    return Result.Failure(Error.Conflict("Nhập lại mật khẩu không đúng"));
                }

                if (!PasswordExtensions.Verify(request.OldPassword, user.PasswordHash))
                {
                    return Result.Failure(Error.Conflict("Mật khẩu cũ không đúng"));
                }

                string newPasswordHash = PasswordExtensions.HashPassword(request.NewPassword);

                user.PasswordHash = newPasswordHash;

                userRepo.Update(user);

                await userRepo.SaveChangesAsync(cancellationToken);

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
