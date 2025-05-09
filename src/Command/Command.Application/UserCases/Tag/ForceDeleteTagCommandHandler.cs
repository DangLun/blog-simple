using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class ForceDeleteTagCommandValidator : AbstractValidator<ForceDeleteTagCommand>
    {
        public ForceDeleteTagCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class ForceDeleteTagCommandHandler : IRequestHandler<ForceDeleteTagCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public ForceDeleteTagCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(ForceDeleteTagCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

                var tag = await tagRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if (tag == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                
                if(tag.PostTags is not null) tag.PostTags.Clear();

                

                tagRepo.Remove(tag);
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
