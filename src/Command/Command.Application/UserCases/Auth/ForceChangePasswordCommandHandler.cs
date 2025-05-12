using Command.Application.Commands.Auth;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class ForceChangePasswordCommandValidator : AbstractValidator<ForceChangePasswordCommand>
    {
        public ForceChangePasswordCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.Email).NotNull();
            RuleFor(x => x.Password).NotNull();
        }
    }
    public class ForceChangePasswordCommandHandler : IRequestHandler<ForceChangePasswordCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceChangePasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceChangePasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var user = await userRepo.FirstOrDefaultAsync(true, x=> x.Id == request.Id && x.Email.Equals(request.Email),
                    cancellationToken);

                if (user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                if (user.IsLoginWithGoogle)
                {
                    return Result.Failure(Error.Conflict("Bạn không thể dùng chức năng này!"));
                }

                string newPasswordHash = PasswordExtensions.HashPassword(request.Password!);

                user.PasswordHash = newPasswordHash;

                userRepo.Update(user);

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
