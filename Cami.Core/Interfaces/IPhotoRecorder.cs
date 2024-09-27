using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface IPhotoRecorder
    {
        EventHandler<PhotoRecordedEventArgs> OnPhotoCreated{ get; set; }
        Task StartRecording(string sourceAddress, CancellationToken token);
    }
}