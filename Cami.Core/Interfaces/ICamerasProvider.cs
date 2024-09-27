using System.Collections.Generic;

namespace Cami.Core.Interfaces
{
    public interface ICamerasProvider
    {
        List<Camera> GetList();
    }
}