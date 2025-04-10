using Command.Application.Abstractions;
using Contract.Constants;
using Microsoft.AspNetCore.Http;

namespace Command.Infrastructure.Services
{
    public class FileService : IFileService
    {
        public async Task<string> UploadFile(IFormFile file, string relativePath)
        {
            try
            {
                Guid guid = Guid.NewGuid();
                string fileName = guid.ToString() + Path.GetExtension(file.FileName);

                string slnDir = Const.GetSolutionDir();
                string dirPath = Path.Combine(slnDir, relativePath);

                if(!Directory.Exists(dirPath)) Directory.CreateDirectory(dirPath);  

                string filePath = Path.Combine(dirPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return fileName;
            }
            catch
            {
                throw;
            }
        }
    }
}
