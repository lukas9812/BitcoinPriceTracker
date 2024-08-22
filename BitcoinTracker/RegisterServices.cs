using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitcoinTracker;

public static class RegisterServices
{
    public static IContainer RegisterAutofacServices(this ContainerBuilder builder, ServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging();

        builder.Populate(serviceCollection);
    
        builder.Register(c =>
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://api.coingecko.com/api/v3/simple/price?ids=bitcoin&vs_currencies=usd")
            };
            return client;
        }).As<HttpClient>().SingleInstance();

        builder.RegisterType<ProcessService>().As<IProcessService>();
        builder.RegisterType<GetApiService>().As<IGetApiService>();
        builder.RegisterType<TriggerService>()
            .As<IHostedService>()
            .SingleInstance();
            
        var container = builder.Build();
        return container;
    }
}