using TennisPlanner.Core.Clients;
using TennisPlanner.Core.Configuration;
using TennisPlanner.Server.Services;
using TennisPlanner.Shared.Services.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<ITennisClient, TennisParisClient>();
builder.Services.AddSingleton<ITransportClient, IdfMobilitesClient>();
builder.Services.AddSingleton<IGeoClient, GeoClient>();
builder.Services.AddSingleton<ILoggerService, LoggerService>(_ => LoggerService.Instance);
builder.Services.AddSingleton<INotificationService, NotificationService>();
#if DEBUG
builder.Services.AddSingleton<IAppConfigurationProvider, LocalConfigurationProvider>();
#else
builder.Services.AddSingleton<IAppConfigurationProvider, AzureConfigurationProvider>();
#endif

builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
