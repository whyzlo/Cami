using System.Collections.Generic;
using System.IO;

namespace Cami.Core
{
    public class PhotoRecordedEventArgs
    {
        public Stream FrameStream { get; }

        public PhotoRecordedEventArgs(
            Stream frameStream
            )
        {
            FrameStream = frameStream;;
        }
    }
}