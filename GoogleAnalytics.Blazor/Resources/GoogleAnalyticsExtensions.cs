using Microsoft.Extensions.DependencyInjection;
using System;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Service collection extensions to add a scoped <see cref="IGBAnalyticsManager"/> service.
/// </summary>
public static class GoogleAnalyticsExtensions
{
    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="trackingId"></param>
    /// <param name="additionalConfigInfo">See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/></param>
    /// <returns></returns>
    public static IServiceCollection AddGBService(this IServiceCollection serviceCollection, Action<GBOptions> configureOptions = null)
    {
        configureOptions ??= options => options = new GBOptions();

        return serviceCollection.AddScoped<IGBAnalyticsManager>(serviceProvider =>
        {
            return ActivatorUtilities.CreateInstance<GBAnalyticsManager>(serviceProvider);
        });
    }
}
