using Command.Application.DTOs.Upload.OutputDTOs;
using Contract.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Command.Application.Commands.Uploads
{
    public class FileUploadCommand : IRequest<Result<FileUploadResponse>>
    {
        public IFormFile? File { get; set; }
    }
}
