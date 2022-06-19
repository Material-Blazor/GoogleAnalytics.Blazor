using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace GoogleAnalytics.Blazor;


/// <inheritdoc/>
public sealed class GBAnalyticsManager : IGBAnalyticsManager
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<GBAnalyticsManager> _logger;

    private bool PerformGlobalTracking { get; set; } = true;
    private bool SuppressPageHits { get; set; } = false;
    private string TrackingId { get; set; } = null;
    private Dictionary<string, object> GlobalConfigData { get; set; } = new();
    private Dictionary<string, object> GlobalEventData { get; set; } = new();
    private bool IsInitialized { get; set; } = false;


    public GBAnalyticsManager(IJSRuntime jsRuntime, NavigationManager navigationManager, ILogger<GBAnalyticsManager> logger)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _logger = logger;
        
        _navigationManager.LocationChanged += OnLocationChanged;
    }


    /// <summary>
    /// Sets the tracking id.
    /// </summary>
    /// <param name="trackingId"></param>
    public void Configure(string trackingId)
    {
        TrackingId = trackingId;

        _ = OnLocationChanged(_navigationManager.Uri);
    }


    private async Task InitializeAsync()
    {
        if (TrackingId == null)
        {
            throw new InvalidOperationException("Invalid TrackingId");
        }

        await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Configure, TrackingId, GlobalConfigData);
        
        IsInitialized = true;

        LogDebugMessage($"[GTAG][{TrackingId}] Configured!");
    }


    /// <inheritdoc/>
    public async Task ConfigureGlobalConfigData(Dictionary<string, object> globalConfigData)
    {
        if (!IsInitialized)
        {
            GlobalConfigData = globalConfigData;

            await InitializeAsync().ConfigureAwait(false);
        }
    }


    /// <inheritdoc/>
    public Task ConfigureGlobalEventData(Dictionary<string, object> globalEventData)
    {
        GlobalEventData = globalEventData;
        return Task.CompletedTask;
    }


    /// <inheritdoc/>
    public async Task TrackNavigation(string uri)
    {
        if (!PerformGlobalTracking)
        {
            return;
        }

        if (!IsInitialized)
        {
            await InitializeAsync().ConfigureAwait(false);
        }

        await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Navigate, TrackingId, uri).ConfigureAwait(false);

        LogDebugMessage($"[GTAG][{TrackingId}] Navigated: '{uri}'");
    }


    /// <inheritdoc/>
    public async Task TrackEvent(string eventName, string eventCategory = null, string eventLabel = null, int? eventValue = null)
    {
        await TrackEvent(eventName, new
        {
            event_category = eventCategory, 
            event_label = eventLabel, 
            value = eventValue
        }).ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public Task TrackEvent(string eventName, int eventValue, string eventCategory = null, string eventLabel = null)
    {
        return TrackEvent(eventName, eventCategory, eventLabel, eventValue);
    }


    /// <inheritdoc/>
    public async Task TrackEvent(string eventName, object eventData)
    {
        if (!PerformGlobalTracking)
        {
            return;
        }

        if (!IsInitialized)
        {
            await InitializeAsync().ConfigureAwait(false);
        }

        await _jsRuntime.InvokeAsync<string>( GoogleAnalyticsInterop.TrackEvent, eventName, eventData, GlobalEventData).ConfigureAwait(false);

        LogDebugMessage($"[GTAG][Event triggered] '{eventName}, {eventData}'");
    }


    /// <inheritdoc/>
    public void EnableGlobalTracking()
    {
        PerformGlobalTracking = true;
    }


    /// <inheritdoc/>
    public void DisableGlobalTracking()
    {
        PerformGlobalTracking = false;
    }


    /// <inheritdoc/>
    public bool IsGlobalTrackingEnabled()
    {
        return PerformGlobalTracking;
    }


    /// <inheritdoc/>
    public void SuppressPageHitTracking()
    {
        SuppressPageHits = true;
    }


    private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
    {
        await OnLocationChanged(args.Location).ConfigureAwait(false);
    }


    private async Task OnLocationChanged(string location)
    {
        if (!string.IsNullOrWhiteSpace(TrackingId) && !SuppressPageHits)
        {
            await TrackNavigation(location);
        }

        SuppressPageHits = false;
    }

    
    private void LogDebugMessage(string message)
    {
        _logger.LogDebug(message);
    }
}
