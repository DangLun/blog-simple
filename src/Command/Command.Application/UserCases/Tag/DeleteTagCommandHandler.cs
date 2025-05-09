using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class DeleteTagCommandValidator : AbstractValidator<DeleteTagCommand>
    {
        public DeleteTagCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
        }
    }
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;

        public DeleteTagCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();

                var tag = await tagRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if (tag == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Tag>(x => x.Id, tag.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                tag.IsDeleted = true;
                tag.UpdatedAt = DateTime.Now;
                tagRepo.Update(tag);
                await unitOfWork.SaveChangesAsync(cancellationToken);

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
