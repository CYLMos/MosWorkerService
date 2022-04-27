using MosWorkerService;
using MosWorkerService.Models;

IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Mos' Worker Service";
    })
    .ConfigureServices((hostContext, services) =>
    {
        IConfiguration configuration = hostContext.Configuration;
        var options = configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

        services.AddSingleton(options);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
