using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cami.Core.Interfaces;

namespace Cami.Core
{
    public class Camera
    {
        public EventHandler<CameraRecordEventArgs> OnCameraFrameRecord;
            
        public Guid Id { get; }
        public string Name { get; }
        private readonly string _address;
        private readonly IPhotoRecorder _photoRecorder;

        public Camera(
            Guid id,
            string name,
            string address,
            IPhotoRecorder photoRecorder
            )
        {
            Id = id;
            Name = name;
            _address = address;
            _photoRecorder = photoRecorder;
        }

        public async Task StartRecordingAsync(CancellationToken cancellationToken = default)
        {
            if (OnCameraFrameRecord != null)
            {
                _photoRecorder.OnPhotoCreated += (_, args) =>
                {
                    OnCameraFrameRecord.Invoke(this, new CameraRecordEventArgs(args.FrameStream, args.StartTime));
                };
            }

            await _photoRecorder.StartRecording(_address, cancellationToken);
        }
    }
}