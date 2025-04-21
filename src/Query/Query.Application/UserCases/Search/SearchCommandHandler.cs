using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Search.Commons;
using Query.Application.DTOs.Search.OutputDTO;
using Query.Application.Query.Search;
using Query.Domain.Abstractions.Repositories;
using Query.Domain.Entities;

namespace Query.Application.UserCases.Search
{
    public class SearchCommandValidator : AbstractValidator<SearchCommand>
    {
        public SearchCommandValidator() {
            RuleFor(x => x.SearchText).NotNull();
        }
    }
    public class SearchCommandHandler : IRequestHandler<SearchCommand, Result<SearchResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public SearchCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<SearchResponseDTO>> Handle(SearchCommand request, CancellationToken cancellationToken)
        {
            var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
            var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

            var posts = postRepo.FindAll();
            var tags = tagRepo.FindAll();
            var users = userRepo.FindAll();

            int totalItems = 0;

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                posts = posts.Where(x => x.PostTitle.Contains(request.SearchText)); 
                tags = tags.Where(x => x.TagName.Contains(request.SearchText)); 
                users = users.Where(x => x.FullName.Contains(request.SearchText));
            }

            if(request.PaginationOptions != null && request.PaginationOptions.SortBy != null
                && request.PaginationOptions.IsDescending != null)
            {
                posts = posts.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);
                tags = tags.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);
                users = users.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending);
            }

            totalItems += posts.Count() + tags.Count() + users.Count();

            var postsPagedList = await PagedList<PostDTO>.CreateAsync(posts.Select(x => new PostDTO
            {
                Id = x.Id,
                PostTitle = x.PostTitle,
                Type = "post",
                TypeVN = "Bài viết"
            }), request.PaginationOptions.Page, request.PaginationOptions.PageSize);
            var tagsPagedList = await PagedList<TagDTO>.CreateAsync(tags.Select(x => new TagDTO
            {
                Id = x.Id,
                TagName = x.TagName,
                Type = "tag",
                TypeVN = "Thẻ"
            }), request.PaginationOptions.Page, request.PaginationOptions.PageSize);
            var usersPagedList = await PagedList<AuthorDTO>.CreateAsync(users.Select(x => new AuthorDTO
            {
                Id = x.Id,
                FullName = x.FullName,
                Type = "author",
                TypeVN = "Tác giả"
            }), request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new SearchResponseDTO
            {
                Authors = usersPagedList,
                Posts = postsPagedList,
                Tags = tagsPagedList,
                TotalItems = totalItems
            });
        }
    }
}
