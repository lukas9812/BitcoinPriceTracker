using Autofac;

using BitcoinTracker;
using BitcoinTracker.Interfaces;
using BitcoinTracker.Jobs;
using BitcoinTracker.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

var builder = new ContainerBuilder();
var hostBuilder = Host.CreateApplicationBuilder(args);

var container = builder.RegisterAutofacServices(hostBuilder);

await using var scope = container.BeginLifetimeScope();
var apiService = scope.Resolve<IGetApiService>();
var processService = scope.Resolve<IProcessService>();
var logger = scope.Resolve<ILogger<HourlyJob>>();
var appSettings = scope.Resolve<IOptions<AppSettings>>();

var job = new HourlyJob(apiService, processService, logger, appSettings);
await job.Trigger();
