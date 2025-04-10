using Command.Application.Command;
using Contract.Errors;
using Contract.Shared;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class VerifyEmailCommandValidator : AbstractValidator<VerifyEmailCommand>
    {
        public VerifyEmailCommandValidator()
        {
            RuleFor(x => x.Token).NotNull().WithMessage("Đường dẫn không hợp lệ");
        }
    }
    public class VerifyEmailCommandHandler : IRequestHandler<VerifyEmailCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public VerifyEmailCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(VerifyEmailCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepository = _unitOfWork.Repository<User, int>();
                var emailTokenRepository = _unitOfWork.Repository<EmailToken, int>();

                EmailToken? token = await emailTokenRepository.FirstOrDefaultAsync(true, x => x.Token == request.Token, cancellationToken,
                    x => x.User);

                if (token is null || token.ExpiredAt < DateTime.Now)
                {
                    return Result.Failure(Error.NotFound("Đường dẫn không tồn tại hoặc đã hết hạn."));
                }

                if (token.User is not null)
                {
                    token.User.IsEmailVerified = true;
                }else Result.Failure(Error.NotFound("Đường dẫn không tồn tại hoặc đã hết hạn."));

                token.IsUsed = true;

                emailTokenRepository.Update(token);

                await emailTokenRepository.SaveChangesAsync(cancellationToken);
                transaction.Commit();
                return Result.Success();
            }
            catch (Exception)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
