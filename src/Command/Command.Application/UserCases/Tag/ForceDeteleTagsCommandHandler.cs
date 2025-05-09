using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class ForceDeteleTagsCommandValidator : AbstractValidator<ForceDeleteTagsCommand>
    {
        public ForceDeteleTagsCommandValidator()
        {
            RuleFor(x => x.Ids).NotNull()
                .WithMessage("Ids không được null")
                .Must(postIds => postIds.Any())
                .WithMessage("Vui lòng chọn trước khi thao tác");
        }
    }
    public class ForceDeteleTagsCommandHandler : IRequestHandler<ForceDeleteTagsCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeteleTagsCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeleteTagsCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
                

                foreach (var id in request.Ids)
                {
                    var tag = await tagRepo.FindByIdAsync(id, true, cancellationToken);
                    if (tag == null)
                    {
                        var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, id);
                        return Result.Failure(Error.NotFound(message));
                    }
                    if (tag.PostTags is not null) tag.PostTags.Clear();
                    tagRepo.Remove(tag);
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
