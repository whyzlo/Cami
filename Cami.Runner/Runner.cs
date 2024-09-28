using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;

namespace Cami.Runner;

public class Runner(ICamerasProvider camerasProvider, IImageStorage imageStorage)
{
    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        var cameras = camerasProvider.GetList();
        var cameraPool = new CameraPool(cameras);

        // TODO: cancellation tokens implementation (separate for separate cam)


        var trackingOrchestrator = new TrackingOrchestrator(cameraPool, imageStorage);
        trackingOrchestrator.Config();
        await trackingOrchestrator.StartAsync(cancellationToken);
    }
}