using Command.Application.Commands.Auth;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Abstractions.Services;
using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class FogotPasswordCommandValidator : AbstractValidator<FogotPasswordCommand>
    {
        public FogotPasswordCommandValidator() {
            RuleFor(x => x.Token).NotNull().MaximumLength(128);
            RuleFor(x => x.NewPassword).NotNull().MaximumLength(128);
        }
    }
    public class FogotPasswordCommandHandler : IRequestHandler<FogotPasswordCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public FogotPasswordCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService) { 
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(FogotPasswordCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var _userRepository = _unitOfWork.Repository<User, int>();
                var _emailTokenRepository = _unitOfWork.Repository<EmailToken, int>();

                var emailToken = await _emailTokenRepository.FirstOrDefaultAsync(true,
                    x => x.Token.Equals(request.Token) && x.ExpiredAt > DateTime.Now && x.IsUsed, cancellationToken);

                if(emailToken is null) {
                    var message = MessageConstant.NotFound<EmailToken>(x => x.Token, request.Token);
                    return Result.Failure(Error.NotFound(message));
                }

                var user = await _userRepository.FirstOrDefaultAsync(true, x => x.Id == emailToken.UserId
                           && x.IsActived, cancellationToken);
                
                if(user is null)
                {
                    var message = MessageConstant.NotFound<User>(x => x.Id, user!.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                // update new password
                user.PasswordHash = PasswordExtensions.HashPassword(request.NewPassword!);
                _userRepository.Update(user);

                // save to db
                await _userRepository.SaveChangesAsync();
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
