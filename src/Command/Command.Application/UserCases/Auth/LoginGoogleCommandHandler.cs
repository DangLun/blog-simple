using Command.Application.Commands.Auth;
using Command.Application.DTOs.Auth.InputDTOs;
using Command.Application.DTOs.Auth.OutputDTOs;
using Command.Domain.Abstractions.Auth;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Abstractions.Services;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Command.Application.UserCases.Auth
{
    public class LoginGoogleCommandValidator : AbstractValidator<LoginGoogleCommand>
    {
        public LoginGoogleCommandValidator()
        {
            RuleFor(x => x.GoogleAccessToken).NotNull().MaximumLength(512);
        }
    }

    public class LoginGoogleCommandHandler : IRequestHandler<LoginGoogleCommand, Result<LoginResponseDTO>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;
        private readonly IGoogleAuthSettings _googleAuthSettings;

        public LoginGoogleCommandHandler(
            IUnitOfWork unitOfWork,
            IJwtProvider jwtProvider,
            IGoogleAuthSettings googleAuthSettings)
        {
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
            _googleAuthSettings = googleAuthSettings;
        }

        public async Task<Result<LoginResponseDTO>> Handle(LoginGoogleCommand request, CancellationToken cancellationToken)
        {
            var googleUserInfoEndpoint = _googleAuthSettings.UserInfoEndpoint;
            if (string.IsNullOrEmpty(googleUserInfoEndpoint))
            {
                return Result.Failure(Error.NotFound("Không tồn tại GoogleAuth trong appSettings"));
            }

            var _userRepository = _unitOfWork.Repository<User, int>();
            var _roleRepository = _unitOfWork.Repository<Role, int>();
            var _refreshTokenRepository = _unitOfWork.Repository<RefreshToken, int>();
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken); 
            try
            {
                using var client = new HttpClient();
                try
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", request.GoogleAccessToken);
                    var response = await client.GetAsync(googleUserInfoEndpoint, cancellationToken);

                    if (!response.IsSuccessStatusCode)
                    {
                        return Result.Failure(Error.NotFound("AccessToken Google không hợp lệ hoặc đã hết hạn"));
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var userInfo = JsonSerializer.Deserialize<GoogleUserInfoDTO>(json);

                    var user = await _userRepository.FirstOrDefaultAsync(true, x => x.Email.Equals(userInfo.email) && !x.IsDeleted, cancellationToken);
                    
                    // check user có email và chưa bị xóa có tồn tại không ?
                    if(user is not null)
                    {
                        // nếu có tài khoản nhưng chưa đăng nhập bằng google trước đó -> tài khoản tạo bình thường
                        if(!user.IsLoginWithGoogle)
                        {
                            return Result.Failure(Error.NotFound("Tài khoản đã tồn tại trên hệ thống!"));
                        }

                        // nếu có tài khoản mà đã đăng nhập bằng google trước đó

                        user.LastLoginAt = DateTime.Now;
                        _userRepository.Update(user);
                        await _userRepository.SaveChangesAsync(cancellationToken);
                        var newAccessToken = _jwtProvider.GenerateAccessToken(user);
                        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

                        var newRefreshTokenEntity = new RefreshToken
                        {
                            Token = newRefreshToken,
                            ExpirationDate = DateTime.Now.AddDays(JwtConst.EXPIRED_REFRESH_TOKEN_DAY),
                            IsRevoked = false,
                            UserId = user.Id,
                        };

                        _refreshTokenRepository.Add(newRefreshTokenEntity);
                        await _refreshTokenRepository.SaveChangesAsync();

                        transaction.Commit();
                        return Result.Success(new LoginResponseDTO
                        {
                            AccessToken = newAccessToken,
                            RefreshToken = newRefreshToken
                        });
                    }
                    // chưa đăng nhập bằng google hay tài khoản bình thường bao giờ -> tạo mới tài khoản đăng nhập bằng google
                    var roleUser = await _roleRepository.FirstOrDefaultAsync(false, x => x.RoleName.ToLower().Equals("user"), cancellationToken);

                    var userEntity = new User
                    {
                        FullName = userInfo.name,
                        Avatar = userInfo.picture,
                        CreatedAt = DateTime.Now,
                        Email = userInfo.email,
                        IsActived = true,
                        IsDeleted = false,
                        IsEmailVerified = true,
                        IsLoginWithGoogle = true,
                        LastLoginAt = DateTime.Now,
                        RoleId = roleUser.Id
                    };

                    _userRepository.Add(userEntity);
                    await _userRepository.SaveChangesAsync(cancellationToken);

                    var accessToken = _jwtProvider.GenerateAccessToken(userEntity);
                    var refreshToken = _jwtProvider.GenerateRefreshToken();

                    var refreshTokenEntity = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpirationDate = DateTime.Now.AddDays(JwtConst.EXPIRED_REFRESH_TOKEN_DAY),
                        IsRevoked = false,
                        UserId = userEntity.Id,
                    };

                    _refreshTokenRepository.Add(refreshTokenEntity);
                    await _refreshTokenRepository.SaveChangesAsync();

                    var responseDto = new LoginResponseDTO
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken
                    };

                    transaction.Commit();
                    return Result.Success(responseDto);
                }catch (Exception ex)
                {
                    return Result.Failure(Error.ServerError(ex.Message));  
                }
            }
            catch (Exception ex)
            {
                return Result.Failure(Error.ServerError(ex.Message));
            }
        }
    }
}