using Command.Application.Commands.Auth;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Abstractions.Services;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class FogotPasswordSendVerifyEmailCommandValidator : AbstractValidator<FogotPasswordSendVerifyEmailCommand>
    {
        public FogotPasswordSendVerifyEmailCommandValidator() {
            RuleFor(x => x.Email).NotNull().EmailAddress().MaximumLength(64);
        }
    }
    public class FogotPasswordSendVerifyEmailCommandHandler : IRequestHandler<FogotPasswordSendVerifyEmailCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public FogotPasswordSendVerifyEmailCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService) { 
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }
        public async Task<Result> Handle(FogotPasswordSendVerifyEmailCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var _userRepository = _unitOfWork.Repository<User, int>();
                var emailTokenRepository = _unitOfWork.Repository<EmailToken, int>();

                var user = await _userRepository.FirstOrDefaultAsync(true, x => x.Email.Equals(request.Email)
                            && x.IsActived, cancellationToken);
                
                if(user is null)
                {
                    var message = MessageConstant.NotFound<User>(x => x.Email, request.Email);
                    return Result.Failure(Error.NotFound(message));
                }

                var emailTokenEntity = new EmailToken
                {
                    Token = Guid.NewGuid().ToString(),
                    ExpiredAt = DateTime.Now.AddMinutes(EmailConst.EXPIRED_AT_MINUTE),
                    UserId = user.Id,
                };
                emailTokenRepository.Add(emailTokenEntity);
                await emailTokenRepository.SaveChangesAsync();

                // send mail
                var routeTo = "fogotpw-verify-email";
                var verificationLink = _emailService.GenerateTokenLink(emailTokenEntity.Token, routeTo);
                await _emailService.SendEmailAsync(request.Email, "Lấy lại mật khẩu", $"<p>Bạn hãy nhấn vào link sau để lấy lại mật khẩu của bạn: <a href='{verificationLink}'>Nhấn vào đây</a></p>");
                
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
