using Command.Application.Commands.Auth;
using Contract.Abstractions.Services;
using Contract.Constants;
using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Constants;
using Command.Domain.Entities;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator() {
            RuleFor(x => x.FullName).NotNull().MaximumLength(64).WithMessage("Độ dài họ tên không quá 64 ký tự");
            RuleFor(x => x.Email).NotNull().MaximumLength(64).WithMessage("Độ dài email không quá 64 ký tự");
            RuleFor(x => x.Password).NotNull().MaximumLength(32).WithMessage("Độ dài mật khẩu không quá 64 ký tự");
        }
    }
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        public RegisterCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;   
        }
        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepository = _unitOfWork.Repository<User, int>();
                var roleRepository = _unitOfWork.Repository<Role, int>();
                var emailTokenRepository = _unitOfWork.Repository<EmailToken, int>();

                var existUserHasGoogle = await userRepository.FirstOrDefaultAsync(false, x => x.Email == request.Email && x.IsLoginWithGoogle, cancellationToken);
                var existUser = await userRepository.FirstOrDefaultAsync(true, x => x.Email == request.Email && x.IsEmailVerified, cancellationToken);
                if(existUserHasGoogle is not null || existUser is not null) {
                    return Result.Failure(Error.Conflict("Email đã đăng ký tài khoản tại website."));
                }

                var userRole = await roleRepository.FirstOrDefaultAsync(true, x => x.RoleName == RoleConst.ROLE_NAME_DEFAULT);

                var user = new User
                {
                    Avatar = UserConst.DEFAULT_AVATAR,
                    Bio = string.Empty,
                    Email = request.Email,
                    FullName = request.FullName,
                    IsLoginWithGoogle = UserConst.DEFAULT_LOGIN_GOOGLE,
                    RoleId = userRole!.Id,
                    PasswordHash = PasswordExtensions.HashPassword(request.Password),
                    CreatedAt = DateTime.UtcNow,
                    IsActived = UserConst.DEFAULT_IS_ACTIVED,
                    IsEmailVerified = false
                };

                userRepository.Add(user);
                await userRepository.SaveChangesAsync(cancellationToken);

                // send mail

                var emailTokenEntity = new EmailToken
                {
                    Token = Guid.NewGuid().ToString(),
                    ExpiredAt = DateTime.Now.AddMinutes(EmailConst.EXPIRED_AT_MINUTE),
                    UserId = user.Id,
                };
                emailTokenRepository.Add(emailTokenEntity);
                await emailTokenRepository.SaveChangesAsync();


                var routeTo = "register-verify-email";
                var verificationLink = _emailService.GenerateTokenLink(emailTokenEntity.Token, routeTo);
                await _emailService.SendEmailAsync(request.Email, "Xác nhận tài khoản đăng ký tại BLOGWEBSITE", $"<p>Bạn hãy nhấn vào link sau để thực hiện xác thực tài khoản: <a href='{verificationLink}'>Nhấn vào đây</a></p>");

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
