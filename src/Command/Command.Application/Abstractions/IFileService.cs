using Microsoft.AspNetCore.Http;

namespace Command.Application.Abstractions
{
    public interface IFileService
    {
        // upload file and return fileName
        Task<string> UploadFile(IFormFile file, string directoryPath);
    }
}
