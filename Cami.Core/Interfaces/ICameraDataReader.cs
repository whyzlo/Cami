using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface ICameraDataReader
    {
        EventHandler<ImageRecordCreatedEventArgs> OnImageRecordCreated{ get; set; }
        Task StartCapturing(string sourceAddress, int fps, CancellationToken token);
    }
}