using Command.Application.Commands.Auth;
using Contract.Errors;
using Contract.Shared;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using FluentValidation;
using FluentValidation.Validators;
using MediatR;

namespace Command.Application.UserCases.Auth
{
    public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
    {
        public LogoutCommandValidator() {
            RuleFor(x => x.AccessToken).NotNull();
            RuleFor(x => x.RefreshToken).NotNull();    
        }
    }
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IBlackListTokenRepository _blackListTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        public LogoutCommandHandler(IRefreshTokenRepository refreshTokenRepository, IBlackListTokenRepository blackListTokenRepository, 
            IUnitOfWork unitOfWork)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _blackListTokenRepository = blackListTokenRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var refreshToken = await _refreshTokenRepository.FirstOrDefaultAsync(true, x => x.Token.Equals(request.RefreshToken));
                if (refreshToken == null)
                {
                    return Result.Failure(Error.NotFound(MessageConstant.NotFoundOrUsed<RefreshToken>(x => x.Token, refreshToken)));
                }

                refreshToken.IsRevoked = true;
                await _refreshTokenRepository.SaveChangesAsync();


                var access_token = new BlackListToken
                {
                    Reason = "logout",
                    TokenRevoked = request.AccessToken
                };
                _blackListTokenRepository.Add(access_token);
                await _blackListTokenRepository.SaveChangesAsync();
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
