using Cami.Core.Interfaces;
using Cami.Infra.Mocks;
using Cami.Infra.Services.File;
using Cami.Photo;
using Microsoft.Extensions.DependencyInjection;

namespace Cami.Infra
{
    public static class DependencyInjectionExtensions
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ICamerasProvider, FakeCamerasProvider>();
            services.AddTransient<IImageStorage, ImageStorage>();
            services.AddTransient<IImageContentService, ImageFileStorageService>();
            services.AddTransient<IImageMetaDataService, FakeImageMetaDataService>();
            services.AddTransient<IPhotoRecorder, PhotoRecorder>();
        }
    }
}