using System;
using Cami.Core.Interfaces;

namespace Cami.Photo
{
    public class PhotoRecorderSettings
    {
        public Guid CameraId { get; }
        public string SourceAddress { get; }
        public int IntervalInMilliseconds { get; }
        
        
        // TODO: add credentials here
        public PhotoRecorderSettings(
            Guid cameraId,
            string sourceAddress,
            IImageStorage destinationImageStorage,
            int intervalInMilliseconds)
        {
            CameraId = cameraId;
            SourceAddress = sourceAddress;
            IntervalInMilliseconds = intervalInMilliseconds;
        }
    }
}