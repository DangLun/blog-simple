using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.User.Commons;
using Query.Application.DTOs.User.OutputDTO;
using Query.Application.Query.Comment;
using Query.Application.Query.User;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.User
{
    public class GetAllFollowedByUserIdQueryValidator : AbstractValidator<GetAllCommentByUserIdQuery>
    {
        public GetAllFollowedByUserIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }
    public class GetAllFollowedByUserIdQueryHandler : IRequestHandler<GetAllFollowedByUserIdQuery, Result<GetAllFollowedByUserIdResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllFollowedByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllFollowedByUserIdResponseDTO>> Handle(GetAllFollowedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            var followRepo = unitOfWork.Repository<Domain.Entities.Follow, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";
            var user = await userRepo.FindByIdAsync((int)request.UserId, false, cancellationToken);
            if (user == null || !user.IsActived)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserId);
                return Result.Failure(Error.NotFound(message));
            }

            var users = userRepo.FindAll();
            var follows = followRepo.FindAll();

            var userResponses = (from u in users
                                 join f in follows on u.Id equals f.FollowerId
                                 join fu in users on f.FollowedId equals fu.Id
                                 where f.FollowerId == (int)request.UserId
                                 select fu);

            var responseQuery = userResponses.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending)
                .Select(x => new UserDTO
                {
                    Avatar = x.Avatar,
                    Bio = x.Bio,
                    CreatedAt = x.CreatedAt,
                    Email = x.Email,
                    FullName = x.FullName,
                    Id = x.Id,
                    IsActived = x.IsActived,
                    IsEmailVerified = x.IsEmailVerified,
                    IsLoginWithGoogle = x.IsLoginWithGoogle,
                    LastLoginAt = x.LastLoginAt,
                    PasswordHash = x.PasswordHash,
                    UpdatedAt = x.UpdatedAt,
                    Role = new RoleDTO
                    {
                        Id = x.Role.Id,
                        RoleName = x.Role.RoleName
                    }
                });

            var response = await PagedList<UserDTO>.CreateAsync(responseQuery, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllFollowedByUserIdResponseDTO
            {
                User = response,
            });
        }
    }
}
