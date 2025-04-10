using Command.Application.Commands.Comment;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Comment
{
    public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentCommandValidator() {
            RuleFor(x => x.Content).NotNull().WithMessage("Không được để trống nội dung comment");
            RuleFor(x => x.PostId).NotNull().GreaterThan(0);
            RuleFor(x => x.ParentCommentId).NotNull().GreaterThanOrEqualTo(0);   
            RuleFor(x => x.UserId).NotNull().GreaterThan(0);
        }
    }
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {

                var _commentRepository = _unitOfWork.Repository<Domain.Entities.Comment, int>();
                var _blogRepository = _unitOfWork.Repository<Domain.Entities.Post, int>();
                var _userRepository = _unitOfWork.Repository<User, int>();
                var blogExist = await _blogRepository.FindByIdAsync((int)request.PostId, false, cancellationToken);

                if (blogExist is null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.Conflict(message));
                }

                var userExist = await _userRepository.FindByIdAsync((int)request.UserId, false, cancellationToken);

                if (userExist is null)
                {
                    var message = MessageConstant.NotFound<User>(x => x.Id, request.UserId);
                    return Result.Failure(Error.Conflict(message));
                }

                var commentEntity = new Domain.Entities.Comment
                {
                    PostId = (int)request.PostId,
                    UserId = (int)request.UserId,
                    CommentText = request.Content,
                    ParentCommentId = (int)request.ParentCommentId,
                    CreatedAt = DateTime.Now,
                    IsDeleted = false,
                };
                _commentRepository.Add(commentEntity);
                await _commentRepository.SaveChangesAsync();
                transaction.Commit();
                return Result.Success();
            }catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
