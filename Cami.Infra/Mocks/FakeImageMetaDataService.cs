using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cami.Core;
using Cami.Core.Interfaces;

namespace Cami.Infra.Mocks
{
    public class FakeImageMetaDataService : IImageMetaDataService
    {
        public async Task<Guid> Save(Guid cameraId, string contentPath)
        {
            return await Task.FromResult(Guid.NewGuid());
        }
    }
}