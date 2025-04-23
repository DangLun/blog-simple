using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Post.Commons;
using Query.Application.DTOs.Post.OutputDTO;
using Query.Application.Query.Post;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Post
{
    public class GetAllPostSavedByUserIdQueryValidator : AbstractValidator<GetAllPostSavedByUserIdQuery>
    {
        public GetAllPostSavedByUserIdQueryValidator()
        {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }

    public class GetAllPostSavedByUserIdQueryHandler : IRequestHandler<GetAllPostSavedByUserIdQuery, Result<GetAllPostSavedByUserIdResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllPostSavedByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllPostSavedByUserIdResponseDTO>> Handle(GetAllPostSavedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";

            var user = await userRepo.FindByIdAsync((int)request.UserId, false, cancellationToken);

            if(user is null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserId);
                return Result.Failure(Error.NotFound(message));
            }

            var posts = postRepo.FindAll(false, null, 
                x => x.PostTags, x => x.User, x => x.SavedByUsers);

            if (request.IsSaved)
            {
                posts = posts.Where(x => x.SavedByUsers != null
                    && x.SavedByUsers.Any(y => y.UserId == (int)request.UserIdCall && y.IsActived));
            }

            var tags = tagRepo.FindAll();

            if (request.FilterOptions != null && !request.FilterOptions.IncludeDeleted)
            {
                posts = posts.Where(x => !x.IsDeleted);
            }

            if (request.FilterOptions != null && request.FilterOptions.IncludeActived) // isPublic = true
            {
                posts = posts.Where(x => x.IsPublished);
            }
            else if (request.FilterOptions != null && !request.FilterOptions.IncludeActived)
            {
                posts = posts.Where(x => !x.IsPublished);
            }

            var responseQueryable = posts.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending)
                .Select(x => new PostDTO
                {
                    Id = x.Id,
                    CreatedAt = x.CreatedAt,
                    IsDeleted = x.IsDeleted,
                    IsPublished = x.IsPublished,
                    PostSummary = x.PostSummary,
                    PostThumbnail = x.PostThumbnail,
                    PostTextId = x.PostTextId,
                    PostTitle = x.PostTitle,
                    TotalComments = x.TotalComments,
                    TotalReactions = x.TotalReactions,
                    TotalReads = x.TotalReads,
                    UpdatedAt = x.UpdatedAt,
                    IsMine = x.User != null && x.User.Id == request.UserIdCall,
                    IsSaved = x.SavedByUsers != null ? x.SavedByUsers.Any(su => su.UserId == request.UserIdCall) ?
                            x.SavedByUsers.First(pt => pt.UserId == request.UserIdCall).IsActived : false : false,
                    Author = new UserDTO
                    {
                        Id = x.User.Id,
                        Avatar = x.User.Avatar,
                        Email = x.User.Email,
                        FullName = x.User.FullName,
                        IsLoginWithGoogle = x.User.IsLoginWithGoogle
                    },
                    Tags = (request.IsRelationTag != null && request.IsRelationTag) ? tags.Where(t => t.PostTags.Any(p => p.PostId == x.Id) && !t.IsDeleted).Select(x => new TagDTO
                    {
                        Id = x.Id,
                        ClassName = x.ClassName,
                        CreatedAt = x.CreatedAt,
                        Description = x.Description,
                        IsDeleted = x.IsDeleted,
                        TagName = x.TagName,
                        UpdatedAt = x.UpdatedAt,
                    }).ToList() : null
                });

            var responsePagedList = await PagedList<PostDTO>
                .CreateAsync(responseQueryable, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllPostSavedByUserIdResponseDTO
            {
                Posts = responsePagedList,
            });
        }
    }
}
