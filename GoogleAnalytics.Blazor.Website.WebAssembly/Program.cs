using GoogleAnalytics.Blazor;
using GoogleAnalytics.Blazor.Website.WebAssembly;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

var userId = $"userid{DateTime.Now.Ticks}";

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Option 1: add options to services, which are then accessed by the Google Analytics Manager.
//builder.Services.AddOptions<GBOptions>().Configure(options =>
//{
//    options.TrackingId = "UA-111742878-2";
//    options.AdditionalConfigInfo = new Dictionary<string, object>() { { "user_id", userId } };
//    options.GlobalEventParams = new Dictionary<string, object>() { { "user_id", userId } };
//});

//builder.Services.AddGBService();

// Option 2: add options within the call to add the Google Analytics Manager.
builder.Services.AddGBService(options =>
{
    options.TrackingId = "UA-111742878-2";
    options.AdditionalConfigInfo = new Dictionary<string, object>() { { "user_id", userId } };
    options.GlobalEventParams = new Dictionary<string, object>() { { "user_id", userId } };
});

builder.Logging.SetMinimumLevel(LogLevel.Debug);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    //.MinimumLevel.Override("GoogleAnalytics.Blazor", LogEventLevel.Debug)
    .Enrich.FromLogContext()
    .WriteTo.Async(a => a.BrowserConsole(outputTemplate: "{Timestamp:HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}"))
    .CreateLogger();

builder.Logging.AddProvider(new SerilogLoggerProvider());

await builder.Build().RunAsync();
