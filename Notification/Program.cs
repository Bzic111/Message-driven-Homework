using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notification;

Console.OutputEncoding = System.Text.Encoding.UTF8;
CreateHostBuilder(args).Build().Run();
IHostBuilder CreateHostBuilder(string[] args) => 
    Host.CreateDefaultBuilder(args).ConfigureServices((hostContext, services) => 
        services.AddHostedService<Worker>());