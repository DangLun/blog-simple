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
            var user = unitOfWork.Repository<Domain.Entities.User, int>();
            var tag = unitOfWork.Repository<Domain.Entities.Tag, int>();   
            request.PaginationOptions.SortBy ??= "CreatedAt";

            var posts = postRepo.FindAll(false, null, x => x.PostTags, x=> x.User, x=> x.SavedByUsers);
            var follows = follow.FindAll();
            var users = user.FindAll();
            var tags = tag.FindAll();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                posts = posts.Where(p =>
                    p.PostTitle.Contains(request.SearchText));
            }

            if (request.FollowOrRecent != null && 
                request.FollowOrRecent.Equals("follow"))
            {
                posts = (from p in posts
                         join u in users on p.UserId equals u.Id
                         join f in follows on u.Id equals f.FollowedId
                         where f.FollowerId == request.UserIdCall && f.FollowedId == u.Id
                         select p
                             );
            }

            if(request.StatusPublisheds != null && request.StatusPublisheds.Any())
            {
                posts = posts.Where(x => request.StatusPublisheds.Contains(x.IsPublished));
            }

            if (request.StatusDeleteds != null && request.StatusDeleteds.Any())
            {
                posts = posts.Where(x => request.StatusDeleteds.Contains(x.IsDeleted));
            }

            if (request.SortStatus != null && request.CurrentDate != null)
            {
                int diff = (7 + (request.CurrentDate.Value.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime startOfWeek = request.CurrentDate.Value.AddDays(-diff);
                DateTime endOfWeek = startOfWeek.AddDays(6);
                posts = request.SortStatus switch
                {
                    0 => posts.Where(x => x.CreatedAt >= startOfWeek && x.CreatedAt <= endOfWeek),
                    1 => posts.Where(x => x.CreatedAt.Value.Month == request.CurrentDate.Value.Month
                                && x.CreatedAt.Value.Year == request.CurrentDate.Value.Year),
                    2 => posts.Where(x => x.CreatedAt.Value.Year == request.CurrentDate.Value.Year),
                    _ => posts
                };
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
                        IsLoginWithGoogle = x.User.IsLoginWithGoogle,
                        CreatedAt = x.User.CreatedAt
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

            return Result.Success(new GetAllPostResponseDTO
            {
                Posts = responsePagedList,
            });
        }
    }
}
