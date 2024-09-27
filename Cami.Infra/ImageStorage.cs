using System;
using System.IO;
using System.Threading.Tasks;
using Cami.Core.Interfaces;

namespace Cami.Infra
{
    public class ImageStorage: IImageStorage
    {
        private readonly IImageMetaDataService _imageMetaDataService;
        private readonly IImageContentService _imageContentService;

        public ImageStorage(IImageMetaDataService imageMetaDataService, IImageContentService imageContentService)
        {
            _imageMetaDataService = imageMetaDataService;
            _imageContentService = imageContentService;
        }

        public async Task<Guid> SaveAsync(Guid cameraId, string prefix, Stream content, DateTime timestamp)
        {
            var contentPath = await _imageContentService.Upload(prefix, timestamp, content);
            var imageId = await _imageMetaDataService.Save(cameraId, contentPath);

            return imageId;
        }

        public async Task<string> DownloadAsync(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}