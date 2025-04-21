using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.User.OutputDTO;
using Query.Application.Query.User;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.User
{
    public class GetDetailUserQueryValidator : AbstractValidator<GetDetailUserQuery>
    {
        public GetDetailUserQueryValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class GetDetailUserQueryHandler : IRequestHandler<GetDetailUserQuery, Result<GetDetailUserResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDetailUserQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<GetDetailUserResponseDTO>> Handle(GetDetailUserQuery request, CancellationToken cancellationToken)
        {
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
            var followRepo = unitOfWork.Repository<Domain.Entities.Follow, int>();
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();

            var user = await userRepo.FindByIdAsync((int)request.Id, false, cancellationToken, x => x.Role);

            if(user == null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                return Result.Failure(Error.NotFound(message));
            }
            if (request.IsActived != null && !(bool)request.IsActived) 
                // Chọn user không còn hoạt động (xóa mềm)
            {
                user = user.IsActived ? null : user;
            }

            if (user == null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                return Result.Failure(Error.NotFound(message));
            }

            var commenteds = commentRepo.FindAll(false, x => x.UserId == request.Id);
            var followers = followRepo.FindAll(false, x => x.FollowedId == request.Id);
            var posteds = postRepo.FindAll(false, x => x.UserId == request.Id);

            var response = new GetDetailUserResponseDTO
            {
                User = new DTOs.User.Commons.UserDTO
                {
                    Id = user.Id,
                    Avatar = user.Avatar,
                    Bio = user.Bio,
                    CreatedAt = user.CreatedAt,
                    Email = user.Email,
                    FullName = user.FullName,
                    IsActived = user.IsActived,
                    IsEmailVerified = user.IsEmailVerified,
                    IsLoginWithGoogle = user.IsLoginWithGoogle,
                    LastLoginAt = user.LastLoginAt,
                    PasswordHash = user.PasswordHash,
                    Role = new DTOs.User.Commons.RoleDTO
                    {
                        Id = user.Role.Id,
                        RoleName = user.Role.RoleName,
                    },
                    UpdatedAt = user.UpdatedAt,
                },
                TotalCommented = commenteds != null ? commenteds.Count() : 0,
                TotalFollower = followers != null ? followers.Count() : 0,
                TotalPosted = posteds != null ? posteds.Count() : 0,
                IsMe = request.UserIdCall == request.Id,
                IsFollowed = await followRepo.FirstOrDefaultAsync(false, 
                        x => x.FollowerId == request.UserIdCall &&
                        x.FollowedId == user.Id, cancellationToken) != null,
            };

            return Result.Success(response);
        }
    }
}
