using Autofac;
using Autofac.Extensions.DependencyInjection;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Services;
using Hangfire;
using Hangfire.MemoryStorage;
using Microsoft.Extensions.DependencyInjection;

namespace BitcoinTracker;

public static class RegisterServices
{
    public static IContainer RegisterAutofacServices(this ContainerBuilder builder, ServiceCollection services)
    {
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