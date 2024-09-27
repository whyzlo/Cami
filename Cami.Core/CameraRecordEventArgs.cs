using System;
using System.Collections.Generic;
using System.IO;

namespace Cami.Core
{
    public class CameraRecordEventArgs
    {
        public Stream FrameStream { get; }
        public DateTime StartTime { get; }

        public CameraRecordEventArgs(Stream frameStream, DateTime startTime)
        {
            FrameStream = frameStream;
            StartTime = startTime;
        }
    }
}