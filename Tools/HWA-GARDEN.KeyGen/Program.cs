// See https://aka.ms/new-console-template for more information
using HWA_GARDEN.KeyGen;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDataProtection();
        services.AddHostedService<KeyGen>();
    })
    .Build()
    .RunAsync();
