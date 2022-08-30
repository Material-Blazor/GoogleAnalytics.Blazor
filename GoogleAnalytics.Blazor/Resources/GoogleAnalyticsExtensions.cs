using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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
    /// <returns></returns>
    public static IServiceCollection AddGBService(this IServiceCollection serviceCollection)
    {
        return serviceCollection.AddScoped<IGBAnalyticsManager>(serviceProvider =>
        {
            return ActivatorUtilities.CreateInstance<GBAnalyticsManager>(serviceProvider, serviceProvider.GetRequiredService<IOptions<GBOptions>>());
        });
    }


    /// <summary>
    /// Adds a scoped service to manage Google Analytics.
    /// </summary>
    /// <param name="serviceCollection"></param>
    /// <param name="configureOptions"></param>
    /// <returns></returns>
    public static IServiceCollection AddGBService(this IServiceCollection serviceCollection, Action<GBOptions> configureOptions)
    {
        GBOptions options = new();

        configureOptions.Invoke(options);

        return serviceCollection.AddScoped<IGBAnalyticsManager>(serviceProvider =>
        {
            return ActivatorUtilities.CreateInstance<GBAnalyticsManager>(serviceProvider, options);
        });
    }
}
