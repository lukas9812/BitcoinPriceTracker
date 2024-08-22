using Autofac;

using BitcoinTracker;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Jobs;
using Microsoft.Extensions.DependencyInjection;

var builder = new ContainerBuilder();
var serviceCollection = new ServiceCollection();

var container = builder.RegisterAutofacServices(serviceCollection);

await using var scope = container.BeginLifetimeScope();
var apiService = scope.Resolve<IGetApiService>();
var processService = scope.Resolve<IProcessService>();

var job = new HourlyJob(apiService, processService);
await job.Trigger();
