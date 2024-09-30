using System;
using System.Threading;
using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;
using OpenCvSharp;

namespace Cami.Capture
{
    public class IpCameraOpenCvStreamReader: ICameraDataReader
    {
        public EventHandler<ImageRecordCreatedEventArgs> OnImageRecordCreated { get; set; }

        public async Task StartCapturing(string ipCameraUrl, int fps, CancellationToken cancellationToken = default)
        {
            var frameInterval = 1000 / fps;
            
            Console.WriteLine("OpenCV Version: " + OpenCvSharp.Cv2.GetVersionString());

            await Task.Run(async () =>
            {
                // TODO: pass ipcamera url here and check if it works as currently it works with local camera and files :(
                //using (var capture = new VideoCapture(0))
                using (var capture = new VideoCapture(ipCameraUrl))
                {
                    var isOpen = capture.IsOpened();
                  if (!capture.IsOpened())
                    {
                        throw new Exception($"Video capture for '{ipCameraUrl}' is closed");
                    }

                    using (var frame = new Mat())
                    {
                        while (true)
                        {
                            if (cancellationToken.IsCancellationRequested)
                            {
                                break;
                            }
                            var startTime = DateTime.UtcNow; 
                            
                            capture.Read(frame);
                            
                            var elapsedTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
                            var remainingDelay = frameInterval - (int)elapsedTime;
                            Console.WriteLine($"Sleeping: { remainingDelay }ms");

                            if (frame.Empty())
                            {
                                await Task.Delay(remainingDelay, cancellationToken);
                            }
                            else
                            {
                                OnImageRecordCreated?.Invoke(this,
                                    new ImageRecordCreatedEventArgs(frame.ToMemoryStream(), startTime));
                                if (remainingDelay > 0)
                                {
                                    await Task.Delay(remainingDelay, cancellationToken);
                                }
                            }
                        }
                    }
                } 
            }, cancellationToken);
        }
    }
}