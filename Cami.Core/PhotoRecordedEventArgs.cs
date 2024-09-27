using System;
using System.Collections.Generic;
using System.IO;

namespace Cami.Core
{
    public class PhotoRecordedEventArgs
    {
        public DateTime StartTime { get; }
        public Stream FrameStream { get; }

        public PhotoRecordedEventArgs(Stream frameStream, DateTime startTime)
        {
            FrameStream = frameStream;
            StartTime = startTime;
        }
    }
}
