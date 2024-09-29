using System.Collections.Generic;
using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;

namespace Cami.Runner;

public class TrackingOrchestrator(CameraPool cameraPool, IImageStorage imageStorage)
{
    public void Config()
    {
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cameraTasks = new List<Task>();
        foreach (var camera in cameraPool.Cameras)
        {
            camera.OnCameraFrameRecord += OnCameraFrameRecordHandler;

            var task = camera.StartRecordingAsync(cancellationToken);
            cameraTasks.Add(task);
        }

        await Task.WhenAll(cameraTasks);
    }

    private void OnCameraFrameRecordHandler(object? sender, CameraRecordEventArgs args)
    {
        if (sender is Camera camera)
        {
            Task.Run(async () => await imageStorage.SaveAsync(camera.Id, camera.Name, args.FrameStream, args.StartTime));
        }
    }
}
