using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cami.Core
{
    public class CameraPool
    {
        public CameraPool(List<Camera> cameras)
        {
            Cameras = cameras;
        }

        public List<Camera> Cameras { get; }

        public async Task StartRecordingAsync()
        {
            throw new NotImplementedException();
        }
    }
}