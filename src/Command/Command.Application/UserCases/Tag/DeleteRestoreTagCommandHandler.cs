using Command.Application.Commands.Post;
using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class DeleteRestoreTagCommandValidator : AbstractValidator<DeleteRestoreTagCommand>
    {
        public DeleteRestoreTagCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.NewStatus).NotNull();
        }
    }
    public class DeleteRestoreTagCommandHandler : IRequestHandler<DeleteRestoreTagCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public DeleteRestoreTagCommandHandler(IUnitOfWork unitOfWork) {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteRestoreTagCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

                var tag = await tagRepo.FindByIdAsync((int)request.Id, true, cancellationToken);
                if(tag == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                tag.IsDeleted = (bool)request.NewStatus;
                tagRepo.Update(tag);  
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
