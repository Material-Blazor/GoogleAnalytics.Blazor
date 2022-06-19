using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Service collection extensions to add a scoped <see cref="IGBAnalyticsManager"/> service.
/// </summary>
public static class GoogleAnalyticsExtensions
{
    /// <summary>
    /// Adds a scoped service to manage Google Analytics. A tracking
    /// id is set from configuration["GoogleAnalytics.Blazor:TrackingId"] if set.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services) => AddGoogleAnalytics(services, null);


    /// <summary>
    /// Adds a scoped service to manage Google Analytics. If the tracking id is null or whitespace, a tracking
    /// id is set from configuration["GoogleAnalytics.Blazor:TrackingId"] if set.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="trackingId"></param>
    /// <param name="debug"></param>
    /// <returns></returns>
    public static IServiceCollection AddGoogleAnalytics(this IServiceCollection services, string trackingId)
    {
        return services.AddScoped<IGBAnalyticsManager>(p =>
        {
            var googleAnalytics = ActivatorUtilities.CreateInstance<GBAnalyticsManager>(p);

            var tiSection = services.BuildServiceProvider().GetService<IConfiguration>().GetSection("GoogleAnalytics.Blazor:TrackingId");

            if (string.IsNullOrWhiteSpace(trackingId))
            {
                trackingId = tiSection.Value;
            }

            if (!string.IsNullOrWhiteSpace(trackingId))
            {
                googleAnalytics.Configure(trackingId);
            }

            return googleAnalytics;
        });
    }
}
