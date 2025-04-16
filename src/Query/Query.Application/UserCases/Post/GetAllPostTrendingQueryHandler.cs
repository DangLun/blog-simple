using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Query.Application.DTOs.Post.Commons;
using Query.Application.DTOs.Post.OutputDTO;
using Query.Application.Query.Post;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Post
{
    public class GetAllPostTrendingQueryHandler : IRequestHandler<GetAllPostTrendingQuery, Result<GetAllPostTrendingResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllPostTrendingQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllPostTrendingResponseDTO>> Handle(GetAllPostTrendingQuery request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

            string sortBy = "TotalReads";
            bool isDesc = true;

            var posts = postRepo.FindAll(false, null, x => x.PostTags, x => x.User, x=> x.SavedByUsers);
            var tags = tagRepo.FindAll();

            posts = posts.Sort(sortBy, isDesc);

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

            if (request.SkipTakeOptions != null && request.SkipTakeOptions.Skip != null &&
                request.SkipTakeOptions.Take != null)
            {
                posts = posts.Skip((int)request.SkipTakeOptions.Skip).Take((int)request.SkipTakeOptions.Take);
            }

            var response = await posts.Select(x => new PostDTO
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
                IsSaved = x.SavedByUsers.Any(su => su.UserId == request.UserIdCall) ?
                            x.SavedByUsers.First(pt => pt.UserId == request.UserIdCall).IsActived : false,
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
            }).ToListAsync();

            return Result.Success(new GetAllPostTrendingResponseDTO
            {
                Posts = response
            });
        }
    }
}
