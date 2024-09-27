using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    // TODO: REMOVE?
    public interface IObjectTracker
    {
        Camera CurrentCamera { get; }
        List<TrackableObjectDescription> TrackableObjectDescriptions { get; }
        Task StartTrackingAsync();
    }
}