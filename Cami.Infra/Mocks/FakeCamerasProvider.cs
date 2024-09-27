using System;
using System.Collections.Generic;
using Cami.Core;
using Cami.Core.Interfaces;
using Cami.Photo;
using Microsoft.Extensions.DependencyInjection;

namespace Cami.Infra.Mocks
{
    public class FakeCamerasProvider : ICamerasProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public FakeCamerasProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public List<Camera> GetList()
        {
            return new List<Camera>
            {
                CreateCamera(new Guid("42d5a6cd-fbeb-4edf-8d58-52fc48a5460a"), "France", "http://185.194.123.84:8001/mjpg/video.mjpg"), 
                CreateCamera(new Guid("95704f48-c9c1-4cd8-982d-893b06dbd4e2"), "Serbia", "http://93.87.72.254:8090/mjpg/video.mjpg")
            };
        }
        
        private Camera CreateCamera(Guid id, string name, string url)
        {
            // Resolve a new instance of IPhotoRecorder from the DI container
            var photoRecorder = _serviceProvider.GetRequiredService<IPhotoRecorder>();

            return new Camera(id, name, url, photoRecorder);
        }
    }
}