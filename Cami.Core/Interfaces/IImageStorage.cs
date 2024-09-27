using System;
using System.IO;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface IImageStorage
    {
        Task<Guid> SaveAsync(Guid cameraId, string prefix, Stream content);
        
        Task<string> DownloadAsync(string name);
    }
}