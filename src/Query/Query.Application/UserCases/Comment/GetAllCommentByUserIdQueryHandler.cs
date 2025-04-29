using Contract.Errors;
using Contract.Extensions;
using Contract.Shared;
using FluentValidation;
using MediatR;
using Query.Application.DTOs.Comment.Commons;
using Query.Application.DTOs.Comment.OutputDTOs;
using Query.Application.Query.Comment;
using Query.Domain.Abstractions.Repositories;

namespace Query.Application.UserCases.Comment
{
    public class GetAllCommentByUserIdQueryValidator : AbstractValidator<GetAllCommentByUserIdQuery>
    {
        public GetAllCommentByUserIdQueryValidator() {
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }
    public class GetAllCommentByUserIdQueryHandler : IRequestHandler<GetAllCommentByUserIdQuery, Result<GetAllCommentByUserIdResponseDTO>>
    {
        private readonly IUnitOfWork unitOfWork;
        public GetAllCommentByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result<GetAllCommentByUserIdResponseDTO>> Handle(GetAllCommentByUserIdQuery request, CancellationToken cancellationToken)
        {
            var commentRepo = unitOfWork.Repository<Domain.Entities.Comment, int>();
            var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
            request.PaginationOptions.SortBy ??= "CreatedAt";
            var user = await userRepo.FindByIdAsync((int)request.UserId, false, cancellationToken); 
            if(user == null)
            {
                var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserId);
                return Result.Failure(Error.NotFound(message));
            }

            var comments = commentRepo.FindAll(false, x => x.User.Id == (int)request.UserId, x => x.User);

            if(request.FilterOptions != null && request.FilterOptions.IncludeDeleted == false)
            {
                comments = comments.Where(x => !x.IsDeleted);
            }

            var responseQuery = comments.Sort(request.PaginationOptions.SortBy, request.PaginationOptions.IsDescending)
                .Select(x => new CommentDTO
                {
                    CommentText = x.CommentText,
                    CreatedAt = x.CreatedAt,
                    IsDeleted = x.IsDeleted,
                    ParentCommentId = x.ParentCommentId,
                    UpdatedAt = x.UpdatedAt,
                    User = new UserDTO
                    {
                        Avatar = x.User.Avatar,
                        CreatedAt = x.User.CreatedAt,
                        Email = x.User.Email,
                        FullName = x.User.FullName,
                        Id = x.User.Id,
                        IsLoginWithGoogle = x.User.IsLoginWithGoogle
                    } 
                });

            var response = await PagedList<CommentDTO>.CreateAsync(responseQuery, request.PaginationOptions.Page, request.PaginationOptions.PageSize);

            return Result.Success(new GetAllCommentByUserIdResponseDTO
            {
                Comments = response
            });
            
        }
    }
}
