using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using TennisPlanner.App;
using TennisPlanner.App.Services;
using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Configuration;
using TennisPlanner.Shared.Services.Logging;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

IAppConfigurationProvider configurationProvider;
#if DEBUG
configurationProvider = new LocalConfigurationProvider();
#else
// TODO ADD PROD FLAG
configurationProvider = new ProdConfigurationProvider(ProductionEnvironment.Canary);
#endif
builder.Services.AddScoped<HttpClient>(sp => new HttpClient { BaseAddress = new Uri(configurationProvider.GetApiBaseUrl()) });
builder.Services.AddScoped<IAppConfigurationProvider>(_ => configurationProvider);
builder.Services.AddScoped<IGeoClient, GeoClient>();
builder.Services.AddScoped<ILoggerService, LoggerService>(_ => LoggerService.Instance);
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<ISearchResultDataProvider, SearchResultDataProvider>();
builder.Services.AddScoped<ISearchFiltersService, SearchFiltersService>();
builder.Services.AddScoped<ITennisPlannerAPIService, TennisPlannerAPIService>();

builder.Logging.SetMinimumLevel(LogLevel.Debug);

await builder.Build().RunAsync();
