using Command.Application.Abstractions;
using Command.Application.Commands.User;
using Command.Domain.Abstractions.Repositories;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using FluentValidation;
using MediatR;

namespace Command.Application.UserCases.User
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator() {
            RuleFor(x => x.Id).NotNull().GreaterThan(0);
            RuleFor(x => x.FullName).NotNull();
            RuleFor(x => x.Email).NotNull();
        }
    }
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Result>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFileService fileService;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IFileService fileService)
        {
            this.unitOfWork = unitOfWork;
            this.fileService = fileService; 
        }

        public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            using var transaction = await unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userRepo = unitOfWork.Repository<Domain.Entities.User, int>();
                var user = await userRepo.FindByIdAsync((int)request.Id, true, cancellationToken);

                if (user == null)
                {
                    var message = MessageConstant.NotFound<Domain.Entities.User>(x => x.Id, request.Id);
                    return Result.Failure(Error.NotFound(message));
                }

                if (request.Avatar != null)
                {
                    string fileName = await fileService.UploadFile(request.Avatar, Const.UPLOAD_DIRECTORY);
                    user.Avatar = fileName;
                }

                user.UpdatedAt = DateTime.Now;
                user.Bio = request.Bio;
                user.FullName = request.FullName;
                if(request.IsActived != null) user.IsActived = (bool)request.IsActived;
                if(request.RoleId != null) user.RoleId = (int)request.RoleId;

                userRepo.Update(user);

                await userRepo.SaveChangesAsync(cancellationToken);

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
