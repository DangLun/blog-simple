using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class UpdateTagCommandValidator : AbstractValidator<UpdateTagCommand>
    {
        public UpdateTagCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.TagName).NotNull();
        }
    }
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        public UpdateTagCommandHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = unitOfWork.Repository<Domain.Entities.Tag, int>();
                
                var tag = await tagRepo.FirstOrDefaultAsync(true, x => x.Id == request.Id, cancellationToken);

                if(tag == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.Comment>(x => x.Id, tag.Id);
                    return Result.Failure(Error.NotFound(message));
                }
                tag.TagName = request.TagName;
                tag.ClassName = request.ClassName;
                tag.IsDeleted = request.IsDeleted ?? tag.IsDeleted;
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
