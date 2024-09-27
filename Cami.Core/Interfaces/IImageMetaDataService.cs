using System;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface IImageMetaDataService
    {
        Task<Guid> Save(Guid cameraId, string contentPath);
    }
}