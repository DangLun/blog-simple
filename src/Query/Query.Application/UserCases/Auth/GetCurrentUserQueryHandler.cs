using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Query.Application.DTOs.Auth.InputDTOs;
using Query.Application.Query.Auth;
using Query.Domain.Abstractions.Auth;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Entities;

namespace Query.Application.UserCases.Auth
{
    public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
    {
        public GetCurrentUserQueryValidator() {
            RuleFor(x => x.AccessToken).NotNull();
        }
    }
    public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, Result<GetCurrentUserDTO>>
    {
        private readonly IJwtProvider _jwtProvider;
        private readonly IUnitOfWork _unitOfWork;
        public GetCurrentUserQueryHandler(IJwtProvider jwtProvider, IUnitOfWork unitOfWork)
        {
            _jwtProvider = jwtProvider;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<GetCurrentUserDTO>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var isValidAccessToken = _jwtProvider.ValidateAccessToken(request.AccessToken!);
            if(!isValidAccessToken) {
                return Result.Failure(Error.ValidationProblem("Token không hợp lệ"));
            }

            var principal = _jwtProvider.GetClaimsPrincipal(request.AccessToken);

            int userId = int.Parse(principal.Claims.First(x => x.Type == "Sub").Value);

            var userRepository = _unitOfWork.Repository<Domain.Entities.User, int>();
            var notificationRepo = _unitOfWork.Repository<Domain.Entities.Notification, int>();
            var user = await userRepository.FindByIdAsync(userId, false, cancellationToken, x => x.Role);

            if(user is null)
            {
                string message = "Người dùng không tồn tại.";
                return Result.Failure(Error.NotFound(message));
            }

            var notSeenMessage = await notificationRepo.FirstOrDefaultAsync(false,
                x => x.RecipientUserId == user.Id && !x.Seen, cancellationToken);


            var response = new GetCurrentUserDTO
            {
                Id = user.Id,
                Avatar = user.Avatar,
                Bio = user.Bio,
                Email = user.Email,
                FullName = user.FullName,
                RoleName = user.Role?.RoleName ?? string.Empty,
                IsLoginGoogle = user.IsLoginWithGoogle,
                HasNewNotification = notSeenMessage != null
            };

            return response;
        }
    }
}
