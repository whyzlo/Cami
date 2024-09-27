using System;
using System.IO;
using System.Threading.Tasks;
using Cami.Core.Interfaces;

namespace Cami.Infra.Services.File
{
    public class ImageFileStorageService : IImageContentService
    {
        // TODO: move this to environment variable
        private readonly string _storagePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "Cami");

        public async Task<string> Upload(string prefix, DateTime timestamp, Stream content)
        {
            var utcTimestamp = timestamp.ToUniversalTime();
            var fileName = $"{prefix}_{utcTimestamp:yyyyMMdd_HHmmss}_{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(_storagePath, fileName);

            Directory.CreateDirectory(_storagePath);

            if (content.CanSeek)
            {
                content.Seek(0, SeekOrigin.Begin);
            }
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await content.CopyToAsync(fileStream);
            }

            return fileName;
        }
    }
}