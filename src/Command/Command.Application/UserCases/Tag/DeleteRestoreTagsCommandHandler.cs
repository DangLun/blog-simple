using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class DeleteRestoreTagsCommandValidator : AbstractValidator<DeleteRestoreTagsCommand>
    {
        public DeleteRestoreTagsCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestoreTagsCommandHandler : IRequestHandler<DeleteRestoreTagsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestoreTagsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestoreTagsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

                foreach(var id in request.Ids)
                {
                    var tag = await tagRepo.FindByIdAsync(id, true, cancellationToken);
                    if (tag == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    tag.IsDeleted = (bool)request.NewStatus;
                    tagRepo.Update(tag);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
                transaction.Commit();
                return Result.Success();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
