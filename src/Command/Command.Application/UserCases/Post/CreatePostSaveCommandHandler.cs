using Command.Application.Commands.Post;
using Command.Domain.Abstractions.Repositories;
using Command.Domain.Entities;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Post
{
    public class CreatePostSaveCommandValidator : AbstractValidator<CreatePostSaveCommand>
    {
        public CreatePostSaveCommandValidator() {
            RuleFor(x => x.PostId).NotNull().GreaterThan(0);
            RuleFor(x => x.UserIdCall).NotNull().GreaterThan(0);
            RuleFor(x => x.Status).NotNull();
        }
    }
    public class CreatePostSaveCommandHandler : IRequestHandler<CreatePostSaveCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public CreatePostSaveCommandHandler(IUnitOfWork unitOfWork) { 
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreatePostSaveCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var postRepo = unitOfWork.Repository<Domain.Entities.Post, int>();
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();

                var post = await postRepo.FindByIdAsync((int)request.PostId, true, cancellationToken);
                var user = await userRepo.FindByIdAsync((int)request.UserIdCall, true, cancellationToken);
                if (post == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Post>(x => x.Id, request.PostId);
                    return Result.Failure(Error.NotFound(message));
                }

                if (user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.UserIdCall);
                    return Result.Failure(Error.NotFound(message));
                }
                var savedRepo = unitOfWork.Repository<PostSaved, int>();

                var savedPost = await savedRepo.FirstOrDefaultAsync(true, x => x.PostId == (int)request.PostId
                    && x.UserId == (int)request.UserIdCall, cancellationToken);

                if (savedPost == null)
                {
                    var savedEntity = new PostSaved
                    {
                        UserId = (int)request.UserIdCall,
                        PostId = (int)request.PostId,
                        IsActived = (bool)request.Status
                    };
                    savedRepo.Add(savedEntity);
                }
                else
                {
                    savedPost.IsActived = !(bool)request.Status;
                    savedRepo.Update(savedPost);
                }

                await savedRepo.SaveChangesAsync(cancellationToken);

                transaction.Commit();

                return Result.Success();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
