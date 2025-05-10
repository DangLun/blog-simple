using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.User.Commons;
using Query.Application.DTOs.User.OutputDTO;
using Query.Application.Query.User;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.User
{
    public class GetAllUserQueryValidator : AbstractValidator<GetAllUserQuery>
    {
        public GetAllUserQueryValidator()
        {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
        }
    }
    public class GetAllUserQueryHandler : IRequestHandler<GetAllUserQuery, Result<GetAllUserResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllUserQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result<GetAllUserResponseDTO>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            var followRepo = unitOfWork.Repository<Domain.Entities.Follow, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";

            var users = userRepo.FindAll(false, null, x => x.Role, x => x.Posts, x => x.Followers);

            if(request.StatusActiveds is not null && request.StatusActiveds.Any())
            {
                users = users.Where(user => request.StatusActiveds.Contains(user.IsActived));
            }

            if(!string.IsNullOrWhiteSpace(request.SearchText))
            {
                users = users.Where(user => user.FullName.ToLower().Contains(request.SearchText.ToLower()));
            }

            var response = users
                .Select(user => new UserGetAllDTO
                {
                    ArticlePublishedCount = user.Posts != null ? user.Posts.Count : 0,
                    Avatar = user.Avatar,
                    Bio = user.Bio,
                    CreatedAt = user.CreatedAt,
                    Email = user.Email,
                    FollowersCount = user.Followers != null ? user.Followers.Count : 0,
                    FullName = user.FullName,
                    Id = user.Id,
                    IsActived = user.IsActived,
                    IsEmailVerified = user.IsEmailVerified,
                    IsLoginWithGoogle = user.IsLoginWithGoogle,
                    LastLoginAt = user.LastLoginAt,
                    PasswordHash = user.PasswordHash,
                    Role = new RoleDTO
                    {
                        Id = user.Role.Id,
                        RoleName = user.Role.RoleName,
                    },
                    UpdatedAt = user.UpdatedAt,
                }).AsEnumerable();

            response = request.PaginationOptions.SortBy switch
            {
                "ArticlePublishedCount" => request.PaginationOptions.IsDescending == true
                    ? response.OrderByDescending(user => user.ArticlePublishedCount)
                    : response.OrderBy(user => user.ArticlePublishedCount),
                "FollowersCount" => request.PaginationOptions.IsDescending == true
                    ? response.OrderByDescending(user => user.FollowersCount)
                    : response.OrderBy(user => user.FollowersCount),
                _ => response.SortEnumerable(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending)
            };

            var userPagedList = PagedList<UserGetAllDTO>.CreateEnumerable(response, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllUserResponseDTO
            {
                User = userPagedList
            });
        }
    }
}
