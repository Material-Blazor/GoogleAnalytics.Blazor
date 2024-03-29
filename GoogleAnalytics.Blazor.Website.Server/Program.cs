using GoogleAnalytics.Blazor;
using GoogleAnalytics.Blazor.Website.Server.Data;
using Serilog;
using Serilog.Events;

var userId = $"userid{DateTime.Now.Ticks}";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, lc) => lc
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .MinimumLevel.Override("GoogleAnalytics.Blazor", LogEventLevel.Debug)
        .Enrich.FromLogContext()
        .WriteTo.Async(a => a.Console(outputTemplate: "{Timestamp:HH:mm:ss.fff}\t[{Level:u3}]\t{Message}{NewLine}{Exception}")));

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Option 1: add options to services, which are then accessed by the Google Analytics Manager.
builder.Services.AddOptions<GBOptions>().Configure(options =>
{
    options.TrackingId = "UA-111742878-2";
    options.AdditionalConfigInfo = new Dictionary<string, object>() { { "user_id", userId } };
    options.GlobalEventParams = new Dictionary<string, object>() { { "user_id", userId } };
});

builder.Services.AddGBService();

// Option 2: add options within the call to add the Google Analytics Manager.
//builder.Services.AddGBService(options =>
//{
//    options.TrackingId = "UA-111742878-2";
//    options.AdditionalConfigInfo = new Dictionary<string, object>() { { "user_id", userId } };
//    options.GlobalEventParams = new Dictionary<string, object>() { { "user_id", userId } };
//});

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
