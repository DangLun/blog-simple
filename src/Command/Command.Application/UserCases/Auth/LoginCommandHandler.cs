using Command.Application.Commands.Auth;
using Command.Application.DTOs.Auth.OutputDTOs;
using Command.Domain.Abstractions.Auth;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Constants;
using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator() {
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtProvider jwtProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public LoginCommandHandler(IUnitOfWork unitOfWork, IJwtProvider jwtProvider, IRefreshTokenRepository refreshTokenRepository)
        {
            this.unitOfWork = unitOfWork;
            this.jwtProvider = jwtProvider;
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<Result<LoginResponseDTO>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepository = unitOfWork.Repository<User, int>();
                var user = await userRepository.FirstOrDefaultAsync(true, x => x.Email == request.Email, cancellationToken);

                if (user is null || !PasswordExtensions.Verify(request.Password, user.PasswordHash))
                {
                    var message = MessageConstant.WrongEmailOrPassword();
                    return Result.Failure(Error.NotFound(message));
                }

                if(!user.IsEmailVerified || user.IsLoginWithGoogle)
                {
                    var message = MessageConstant.WrongEmailOrPassword();
                    return Result.Failure(Error.NotFound(message));
                }

                // set last login at for user
                user.LastLoginAt = DateTime.Now;
                userRepository.Update(user);
                await userRepository.SaveChangesAsync();

                var accessToken = jwtProvider.GenerateAccessToken(user);

                var refreshToken = jwtProvider.GenerateRefreshToken();

                var refreshTokenEntity = new RefreshToken
                {
                    Token = refreshToken,
                    ExpirationDate = DateTime.Now.AddDays(JwtConst.EXPIRED_REFRESH_TOKEN_DAY),
                    IsRevoked = false,
                    UserId = user.Id,
                };
                _refreshTokenRepository.Add(refreshTokenEntity);
                await _refreshTokenRepository.SaveChangesAsync();

                transaction.Commit();

                return Result.Success(new LoginResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                });
            }
            catch(Exception ex)
            {
                transaction.Rollback();
                throw;
            }

        }
    }
}
