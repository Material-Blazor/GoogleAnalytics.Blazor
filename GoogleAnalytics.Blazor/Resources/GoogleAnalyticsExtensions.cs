using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Service collection extensions to add a scoped <see cref="IGBAnalyticsManager"/> service.
/// </summary>
public static class GoogleAnalyticsExtensions
{
    /// <summary>
    /// Adds a scoped service to manage Google Analytics. If the tracking id is null or whitespace, a tracking
    /// id is set from configuration["GoogleAnalytics.Blazor:TrackingId"] if set.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="trackingId"></param>
    /// <param name="additionalConfigInfo">See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/></param>
    /// <returns></returns>
    public static IServiceCollection AddGBService(this IServiceCollection services, string trackingId = null, IDictionary additionalConfigInfo = null)
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
                googleAnalytics.SetTrackingId(trackingId);
            }

            return googleAnalytics;
        });
    }
}
