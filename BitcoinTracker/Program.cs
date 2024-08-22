using Autofac;

using BitcoinTracker;
using BitcoinTracker.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Uwp.Notifications;

var builder = new ContainerBuilder();
var serviceCollection = new ServiceCollection();

// The Microsoft.Extensions.Logging package provides this one-liner
// to add logging services.
var container = builder.RegisterAutofacServices(serviceCollection);

await using var scope = container.BeginLifetimeScope();
var processService = scope.Resolve<IProcessService>();
var tmp = scope.Resolve<IGetApiService>();
var result = await tmp.CallBitcoinApi();

var price = processService.ProcessBitcoinPrice(result);

var imageUri = Path.GetFullPath(@"Images\btc_logo.png");

new ToastContentBuilder()
    .AddInlineImage(new Uri(imageUri))
    .AddAppLogoOverride(new Uri(imageUri))
    .AddText("BITCOIN PRICE NOTIFICATION")
    .AddText($"Current Bitcoin price is: {price} USD.")
    .Show();

