using Command.Application.Commands.Auth;
using Command.Application.DTOs.Auth.OutputDTOs;
using Command.Domain.Abstractions.Auth;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator() {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
    public class RefreshTokenCommandHandler 
        : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IJwtProvider jwtProvider;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IBlackListTokenRepository _blackListTokenRepository;
        public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, 
            IJwtProvider jwtProvider, 
            IRefreshTokenRepository refreshTokenRepository,
            IBlackListTokenRepository blackListTokenRepository)
        {
            this.unitOfWork = unitOfWork;
            this.jwtProvider = jwtProvider;
            _refreshTokenRepository = refreshTokenRepository;
            _blackListTokenRepository = blackListTokenRepository;
        }
        public async Task<Result<RefreshTokenResponseDTO>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = request.RefreshToken;
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepository = unitOfWork.Repository<User, int>();
                
                var refreshTokenEntity = await _refreshTokenRepository.FirstOrDefaultAsync(true,
                    x => x.Token == refreshToken && !x.IsRevoked);
                
                // check isValid refreshTokenEntity 
                if(refreshTokenEntity is null)
                {
                    var message = MessageConstant.NotFoundOrUsed<RefreshToken>(x => x.Token, refreshToken);
                    return Result.Failure(Error.NotFound(message));
                }

                // check token expired
                if(refreshTokenEntity.ExpirationDate < DateTime.UtcNow)
                {
                    var message = MessageConstant.RefreshTokenIsExpired();
                    return Result.Failure(Error.TokenExpired(message));
                }

                // get user got refreshtoken
                var user = await userRepository.FindByIdAsync(refreshTokenEntity.UserId);

                // revoke old refreshToken
                await _refreshTokenRepository.RevokeRefreshToken(refreshTokenEntity);

                // revoke old accessToken
                _blackListTokenRepository.Add(new BlackListToken
                {
                    Reason = "Revoke by used refreshToken",
                    TokenRevoked = request.OldAccessToken
                });
                await _blackListTokenRepository.SaveChangesAsync();

                // Generate new AccessToken and RefreshToken
                var accessToken = jwtProvider.GenerateAccessToken(user!);
                var newRefreshToken = jwtProvider.GenerateRefreshToken();

                // save new refreshToken to database
                var newRefreshTokenEntity = new RefreshToken
                {
                    Token = newRefreshToken,
                    ExpirationDate = DateTime.UtcNow.AddDays(JwtConst.EXPIRED_REFRESH_TOKEN_DAY),
                    IsRevoked = false,
                    UserId = user!.Id
                };

                _refreshTokenRepository.Add(newRefreshTokenEntity);
                await _refreshTokenRepository.SaveChangesAsync();

                transaction.Commit();
                return Result.Success(new RefreshTokenResponseDTO
                {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                });

            }catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
