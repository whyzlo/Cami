using Cami.Infra;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Cami.Runner;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                // Call the extension method to configure services
                services.ConfigureServices();
                services.AddSingleton<Runner>();
            })
            .Build();


        var runner = host.Services.GetRequiredService<Runner>();
        await runner.StartAsync();

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}