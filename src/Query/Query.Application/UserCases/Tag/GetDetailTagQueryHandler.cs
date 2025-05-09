using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Tag.Commons;
using Query.Application.DTOs.Tag.OutputDTOs;
using Query.Application.Query.Tag;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Tag
{
    public class GetDetailTagQueryValidator : AbstractValidator<GetDetailTagQuery>
    {
        public GetDetailTagQueryValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }

    public class GetDetailTagQueryHandler : IRequestHandler<GetDetailTagQuery, Result<GetDetailTagResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetDetailTagQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetDetailTagResponseDTO>> Handle(GetDetailTagQuery request, CancellationToken cancellationToken)
        {

            request.PaginationOptions.SortBy ??= "CreatedAt";

            var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var tag = await tagRepo.FindByIdAsync((int)request.Id, false, cancellationToken, x => x.PostTags);

            if (tag is null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, request.Id);
                return Result.Failure(Error.NotFound(message));
            }

            if (request.FilterOptions != null && !request.FilterOptions.IncludeDeleted)
            {
                tag = tag.IsDeleted ? null : tag;
            }

            if (tag is null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, request.Id);
                return Result.Failure(Error.NotFound(message));
            }

            var tags = tagRepo.FindAll();
            var posts = postRepo.FindAll(false, null, x => x.PostTags, x => x.User, x=> x.SavedByUsers);

            var response = new GetDetailTagResponseDTO
            {
                Id = tag.Id,
                ClassName = tag.ClassName,
                CreatedAt = tag.CreatedAt,
                Description = tag.Description,
                IsDeleted = tag.IsDeleted,
                TagName = tag.TagName,
                UpdatedAt = tag.UpdatedAt,
            };

            if (request.IsRelationPost != null && (bool)request.IsRelationPost)
            {
                IQueryable<PostDTO> postsQuery =
                    from p in posts
                    where p.PostTags.Any(pt => pt.TagId == tag.Id) && p.IsPublished && !p.IsDeleted
                    select new PostDTO
                    {
                        Id = p.Id,
                        CreatedAt = p.CreatedAt,
                        IsDeleted = p.IsDeleted,
                        PostSummary = p.PostSummary,
                        IsPublished = p.IsPublished,
                        PostTextId = p.PostTextId,
                        PostThumbnail = p.PostThumbnail,
                        PostTitle = p.PostTitle,
                        TotalComments = p.TotalComments,
                        TotalReactions = p.TotalReactions,
                        TotalReads = p.TotalReads,
                        UpdatedAt = p.UpdatedAt,
                        IsMine = p.User != null && p.User.Id == request.UserIdCall,
                        IsSaved = p.SavedByUsers != null ? p.SavedByUsers.Any(su => su.UserId == request.UserIdCall) ?
                           p.SavedByUsers.First(pt => pt.UserId == request.UserIdCall).IsActived : false : false,
                        Author = p.User != null ? new UserDTO
                        {
                            Id = p.User.Id,
                            Email = p.User.Email,
                            FullName = p.User.FullName,
                            Avatar = p.User.Avatar,
                            IsLoginWithGoogle = p.User.IsLoginWithGoogle
                        } : null,
                        Tags = (
                            from pt in p.PostTags
                            join t in tags  // IQueryable<Tag> từ EF Core
                                on pt.TagId equals t.Id
                            select new TagDTO
                            {
                                Id = t.Id,
                                TagName = t.TagName,
                                ClassName = t.ClassName,
                                Description = t.Description,
                                IsDeleted = t.IsDeleted,
                                CreatedAt = t.CreatedAt,
                                UpdatedAt = t.UpdatedAt,
                            }
                        ).ToList()
                    };

                postsQuery = postsQuery.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);

                response.Posts = await PagedList<PostDTO>.CreateAsync(postsQuery, 
                    request.PaginationOptions.Page, 
                    request.PaginationOptions.PageSize);
            }
            return Result.Success(response);
        }
    }
}
