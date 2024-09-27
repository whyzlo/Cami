using Cami.Core;
using Cami.Core.Interfaces;

namespace Cami.Runner;

public class Runner(ICamerasProvider camerasProvider, IImageStorage imageStorage)
{
    private readonly ICamerasProvider _camerasProvider = camerasProvider;
    private readonly IImageStorage _imageStorage = imageStorage;

    public async Task StartAsync()
    {
        var cameras = _camerasProvider.GetList();
        var cameraPool = new CameraPool(cameras);

        // TODO: cancellation tokens implementation (separate for separate cam)


        var trackingOrchestrator = new TrackingOrchestrator(cameraPool, _imageStorage);
        trackingOrchestrator.Config();
        await trackingOrchestrator.StartAsync();
    }
}