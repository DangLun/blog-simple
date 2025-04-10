using Command.Application.Abstractions;
using Command.Application.Commands.Uploads;
using Command.Application.DTOs.Upload.OutputDTOs;
using Contract.Constants;
using Contract.Errors;
using Contract.Shared;
using MediatR;

namespace Command.Application.UserCases.Upload
{
    public class UploadFileCommandHandler : IRequestHandler<FileUploadCommand, Result<FileUploadResponse>>
    {
        private readonly IFileService _fileService;
        public UploadFileCommandHandler(IFileService fileService)
        {
            _fileService = fileService;
        }
        public async Task<Result<FileUploadResponse>> Handle(FileUploadCommand request, CancellationToken cancellationToken)
        {

            if(request.File is null)
            {
                return Result.Failure(Error.ValidationProblem("File was null"));
            }

            string fileName = await _fileService.UploadFile(request.File, Const.UPLOAD_DIRECTORY);

            return Result.Success(new FileUploadResponse
            {
                FileName = $"{Const.REQUEST_PATH_STATIC_FILE}/" + fileName
            });
        }
    }
}