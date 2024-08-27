using Autofac;
using Autofac.Extensions.DependencyInjection;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Models;
using BitcoinTracker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace BitcoinTracker;

public static class RegisterServices
{
    public static IContainer RegisterAutofacServices(this ContainerBuilder builder, HostApplicationBuilder hostBuilder)
    {
        var services = new ServiceCollection();
        
        hostBuilder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        services.AddLogging(logBuilder => logBuilder.AddSimpleConsole(options =>
        {
            options.IncludeScopes = true;
            options.SingleLine = true;
            options.TimestampFormat = "HH:mm:ss ";
        }));
        services.Configure<AppSettings>(hostBuilder.Configuration.GetSection("AppSettings"));
        
        builder.Populate(services);

        builder.Register(c =>
        {
            var client = new HttpClient();
            return client;
        }).As<HttpClient>().SingleInstance();

        builder.RegisterType<ProcessService>().As<IProcessService>();
        builder.RegisterType<GetApiService>().As<IGetApiService>();

        var container = builder.Build();
        return container;
    }
}