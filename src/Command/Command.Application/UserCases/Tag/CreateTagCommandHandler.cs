using Command.Application.Commands.Tag;
using Command.Domain.Abstractions.Repositories;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.Tag
{
    public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
    {
        public CreateTagCommandValidator() {
            RuleFor(x => x.TagName).NotNull();
        }
    }
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateTagCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var tagRepo = _unitOfWork.Repository<Domain.Entities.Tag, int>();

                var tag = new Domain.Entities.Tag
                {
                    ClassName = request.ClassName,
                    CreatedAt = DateTime.Now,
                    Description = request.Description,
                    IsDeleted = false,
                    TagName = request.TagName!,
                };

                
                tagRepo.Add(tag);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                
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
