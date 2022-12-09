using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = CreateHostBuilder(args);

IHostBuilder CreateHostBuilder(string[] args)
{
    var host = Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) =>
    {
        services.AddHostedService<Listener>();
    });
    return host;
}

var app = host.Build();
app.Run();
