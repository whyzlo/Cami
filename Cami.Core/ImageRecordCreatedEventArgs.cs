using System;
using System.Collections.Generic;
using System.IO;

namespace Cami.Core
{
    public class ImageRecordCreatedEventArgs
    {
        public DateTime StartTime { get; }
        public Stream ImageStream { get; }

        public ImageRecordCreatedEventArgs(Stream imageStream, DateTime startTime)
        {
            ImageStream = imageStream;
            StartTime = startTime;
        }
    }
}
