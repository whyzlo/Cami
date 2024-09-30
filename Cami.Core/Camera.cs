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
        private readonly ICameraDataReader _cameraDataReader;

        public Camera(
            Guid id,
            string name,
            string address,
            ICameraDataReader cameraDataReader
            )
        {
            Id = id;
            Name = name;
            _address = address;
            _cameraDataReader = cameraDataReader;
        }

        public async Task StartRecordingAsync(CancellationToken cancellationToken = default)
        {
            if (OnCameraFrameRecord != null)
            {
                _cameraDataReader.OnImageRecordCreated += (_, args) =>
                {
                    OnCameraFrameRecord.Invoke(this, new CameraRecordEventArgs(args.ImageStream, args.StartTime));
                };
            }

            // TODO: config FPS
            await _cameraDataReader.StartCapturing(_address, fps: 25, cancellationToken);
        }
    }
}