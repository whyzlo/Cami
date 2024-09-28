using System.Collections.Generic;
using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;
using Cami.Recognize;

namespace Cami.Runner;

public class TrackingOrchestrator(CameraPool cameraPool, IImageStorage imageStorage)
{
    public void Config()
    {
    }

    public async Task StartAsync()
    {
        var cameraTasks = new List<Task>();
        foreach (var camera in cameraPool.Cameras)
        {
            camera.OnCameraFrameRecord += OnCameraFrameRecordHandler;

            var task = camera.StartRecordingAsync();
            cameraTasks.Add(task);
        }

        await Task.WhenAll(cameraTasks);
    }

    private void OnCameraFrameRecordHandler(object? sender, CameraRecordEventArgs args)
    {
        if (sender is Camera camera)
        {
            Task.Run(async () => await imageStorage.SaveAsync(camera.Id, camera.Name, args.FrameStream, args.StartTime));

       /*     HaarCascadeAlgorithm r = new HaarCascadeAlgorithm();
            var result = r.GetImageInfo(args.FrameStream); */
        }
    }
}