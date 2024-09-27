using System;
using System.IO;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface IImageContentService
    {
        Task<string> Upload(string prefix, DateTime timestamp, Stream content);
    }
}