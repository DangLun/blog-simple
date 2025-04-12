using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Post.Commons;
using Query.Application.DTOs.Post.OutputDTO;
using Query.Application.Query.Post;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Entities;

namespace Query.Application.UserCases.Post
{
    public class GetAllPostQueryValidator : AbstractValidator<GetAllPostQuery>
    {
        public GetAllPostQueryValidator() {
            RuleFor(x => x.PaginationOptions.Page).GreaterThan(0);
            RuleFor(x => x.PaginationOptions.PageSize).GreaterThan(0);
        }
    }


    public class GetAllPostQueryHandler : IRequestHandler<GetAllPostQuery, Result<GetAllPostResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllPostQueryHandler(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllPostResponseDTO>> Handle(GetAllPostQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var follow = unitOfWork.Repository<Follow, int>();
            var user = unitOfWork.Repository<User, int>();
            var tag = unitOfWork.Repository<Domain.Entities.Tag, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";

            var posts = postRepo.FindAll(false, null, x => x.PostTags, x=> x.User);
            var follows = follow.FindAll();
            var users = user.FindAll();
            var tags = tag.FindAll();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                posts = posts.Where(p =>
                    p.PostTitle.Contains(request.SearchText));
            }

            if (request.FollowOrRecent != null && 
                request.FollowOrRecent.Equals("follow") && request.UserIdCall != null)
            {
                posts = (from p in posts
                         join u in users on p.UserId equals u.Id
                         join f in follows on u.Id equals f.FollowedId
                         where f.FollowerId == request.UserIdCall && f.FollowedId == u.Id
                         select p
                             );
            }


            if(request.FilterOptions != null && !request.FilterOptions.IncludeDeleted)
            {
                posts = posts.Where(x => !x.IsDeleted);
            }

            if(request.FilterOptions != null && request.FilterOptions.IncludeActived) // isPublic = true
            {
                posts = posts.Where(x => x.IsPublished);
            }else if(request.FilterOptions != null && !request.FilterOptions.IncludeActived)
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
                    Author = new UserDTO
                    {
                        Id = x.User.Id,
                        Avatar = x.User.Avatar,
                        Email = x.User.Email,   
                        FullName = x.User.FullName,
                        IsLoginWithGoogle = x.User.IsLoginWithGoogle   
                    },
                    Tags = tags.Where(t => t.PostTags.Any(pt => pt.TagId == t.Id) && !t.IsDeleted).Select(x => new TagDTO
                    {
                        Id = x.Id,
                        ClassName = x.ClassName,
                        CreatedAt = x.CreatedAt,
                        Description = x.Description,
                        IsDeleted = x.IsDeleted,
                        TagName = x.TagName,
                        UpdatedAt = x.UpdatedAt,
                    }).ToList()
                });

            var responsePagedList = await PagedList<PostDTO>
                .CreateAsync(responseQueryable, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllPostResponseDTO
            {
                Posts = responsePagedList,
            });
        }
    }
}
