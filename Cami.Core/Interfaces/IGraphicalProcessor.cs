using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cami.Core.Interfaces
{
    public interface IGraphicalProcessor
    {
        
        Task StartProcessingAsync(string address, List<TrackableObjectDescription> trackableObjectDescriptions);
    }
}