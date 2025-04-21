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
    public class GetAllPostByUserIdQueryValidator : AbstractValidator<GetAllPostByUserIdQuery>
    {
        public GetAllPostByUserIdQueryValidator()
        {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }

    public class GetAllPostByUserIdQueryHandler : IRequestHandler<GetAllPostByUserIdQuery, Result<GetAllPostByUserIdResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllPostByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllPostByUserIdResponseDTO>> Handle(GetAllPostByUserIdQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var user = unitOfWork.Repository<Domain.Entities.User, int>();
            var tag = unitOfWork.Repository<Domain.Entities.Tag, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";

            var currentUser = await user.FindByIdAsync((int)request.UserId, false, cancellationToken);

            if(currentUser is null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserId);
                return Result.Failure(Error.NotFound(message));
            }

            var posts = postRepo.FindAll(false, x => x.User.Id == request.UserId, x => x.PostTags, x => x.User, x=>x.SavedByUsers);
            var users = user.FindAll(false, x => x.Id == request.UserId);
            var tags = tag.FindAll();

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
                    Tags = (request.IsRelationTag != null && (bool)request.IsRelationTag) ? tags.Where(t => t.PostTags.Any(p => p.PostId == x.Id) && !t.IsDeleted).Select(x => new TagDTO
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

            return Result.Success(new GetAllPostByUserIdResponseDTO
            {
                Posts = responsePagedList,
            });
        }
    }
}
