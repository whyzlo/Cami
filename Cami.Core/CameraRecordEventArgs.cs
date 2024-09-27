using System.Collections.Generic;
using System.IO;

namespace Cami.Core
{
    public class CameraRecordEventArgs
    {
        public Stream FrameStream { get; }

        public CameraRecordEventArgs(
            Stream frameStream
            )
        {
            FrameStream = frameStream;;
        }
    }
}