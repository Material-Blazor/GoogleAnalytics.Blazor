using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Service collection extensions to add a scoped <see cref="IGBAnalyticsManager"/> service.
/// </summary>
public static class GoogleAnalyticsExtensions
{
    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="services"></param>
    /// <param name="trackingId"></param>
    /// <param name="additionalConfigInfo">See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/></param>
    /// <returns></returns>
    public static IServiceCollection AddGBService(this IServiceCollection services, string trackingId = null, IDictionary<string, object> additionalConfigInfo = null, IDictionary<string, object> globalEventParams = null)
    {
        return services.AddScoped<IGBAnalyticsManager>(p =>
        {
            var googleAnalytics = ActivatorUtilities.CreateInstance<GBAnalyticsManager>(p);

            if (!string.IsNullOrWhiteSpace(trackingId))
            {
                _ = googleAnalytics.SetTrackingId(trackingId);
            }

            if (additionalConfigInfo != null)
            {
                _ = googleAnalytics.SetAdditionalConfigInfo(additionalConfigInfo);
            }

            if (globalEventParams != null)
            {
                googleAnalytics.SetGlobalEventParams(globalEventParams);
            }

            return googleAnalytics;
        });
    }
}
