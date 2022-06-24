using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace GoogleAnalytics.Blazor;


/// <inheritdoc/>
public sealed class GBAnalyticsManager : IGBAnalyticsManager
{
    private readonly IJSRuntime _jsRuntime;
    private readonly NavigationManager _navigationManager;
    private readonly ILogger<GBAnalyticsManager> _logger;
    private readonly SemaphoreSlim _configurationSemaphore = new(1, 1);
    private readonly SemaphoreSlim _setPropertySemaphore = new(1, 1);

    private bool PerformGlobalTracking { get; set; } = true;
    private bool SuppressNextPageHit { get; set; } = false;
    private string TrackingId { get; set; } = null;
    private ImmutableDictionary<string, object> AdditionalConfigInfo { get; set; } = ImmutableDictionary<string, object>.Empty;
    private ImmutableDictionary<string, object> GlobalEventParams { get; set; } = ImmutableDictionary<string, object>.Empty;
    private bool IsConfigured { get; set; } = false;
    private bool IsInitialized { get; set; } = false;


    public GBAnalyticsManager(IJSRuntime jsRuntime, NavigationManager navigationManager, ILogger<GBAnalyticsManager> logger)
    {
        _jsRuntime = jsRuntime;
        _navigationManager = navigationManager;
        _logger = logger;
        
        _navigationManager.LocationChanged += OnLocationChanged;
    }


    /// <summary>
    /// Initializes the service. No tracking will be performed until this is called by <see cref="GBAnchor"/>;
    /// </summary>
    internal async Task Initialize()
    {
        IsInitialized = true;

        await ConfigureAsync().ConfigureAwait(false);
    }


    /// <inheritdoc/>
    public async Task SetTrackingId(string trackingId)
    {
        if (TrackingId == trackingId)
        {
            return;
        }

        await _setPropertySemaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            TrackingId = trackingId;

            IsConfigured = false;

            await ConfigureAsync().ConfigureAwait(false);
        }
        finally
        {
            _setPropertySemaphore.Release();
        }
    }


    /// <inheritdoc/>
    public string GetTrackingId()
    {
        return TrackingId ?? "";
    }


    /// <inheritdoc/>
    public async Task SetAdditionalConfigInfo(IDictionary<string, object> additionalConfigInfo)
    {
        try
        {
            var newDict = additionalConfigInfo.OrderBy(x => x.Key).ToImmutableDictionary();

            var isNew = newDict.Count != AdditionalConfigInfo.Count;

            if (!isNew)
            {
                using var newEnumerator = newDict.GetEnumerator();
                using var oldEnumerator = AdditionalConfigInfo.GetEnumerator();

                while (newEnumerator.MoveNext())
                {
                    oldEnumerator.MoveNext();

                    if (newEnumerator.Current.Key != oldEnumerator.Current.Key || newEnumerator.Current.Value != oldEnumerator.Current.Value)
                    {
                        isNew = true;
                        break;
                    }
                }
            }

            if (isNew)
            {
                AdditionalConfigInfo = newDict;

                IsConfigured = false;

                await ConfigureAsync().ConfigureAwait(false);
            }
        }
        finally
        {
            _setPropertySemaphore.Release();
        }
    }


    /// <inheritdoc/>
    public ImmutableDictionary<string, object> GetAdditionalConfigInfo()
    {
        return AdditionalConfigInfo;
    }


    /// <inheritdoc/>
    public void SetGlobalEventParams(IDictionary<string, object> globalEventParams)
    {
        GlobalEventParams = globalEventParams.OrderBy(x => x.Key).ToImmutableDictionary();
    }


    /// <inheritdoc/>
    public ImmutableDictionary<string, object> GetGlobalEventParams()
    {
        return GlobalEventParams;
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

        if (!IsConfigured)
        {
            await ConfigureAsync().ConfigureAwait(false);
        }

        await _jsRuntime.InvokeAsync<string>( GoogleAnalyticsInterop.TrackEvent, eventName, eventData, GlobalEventParams).ConfigureAwait(false);

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
    public void SuppressNextPageHitTracking()
    {
        SuppressNextPageHit = true;
    }


    private async Task ConfigureAsync()
    {
        if (!IsInitialized)
        {
            return;
        }

        await _configurationSemaphore.WaitAsync().ConfigureAwait(false);

        try
        {
            if (IsConfigured || string.IsNullOrWhiteSpace(TrackingId))
            {
                return;
            }

            await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Configure, TrackingId, AdditionalConfigInfo);

            IsConfigured = true;

            LogDebugMessage($"[GTAG][{TrackingId}] Configured");

            await TrackNavigation(_navigationManager.Uri).ConfigureAwait(false);
        }
        finally
        {
            _configurationSemaphore.Release();
        }
    }


    private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
    {
        await ConfigureAsync().ConfigureAwait(false);
        
        if (!SuppressNextPageHit)
        {
            await TrackNavigation(args.Location);
        }

        SuppressNextPageHit = false;
    }


    private async Task TrackNavigation(string uri)
    {
        if (!IsConfigured || !PerformGlobalTracking)
        {
            return;
        }

        await _jsRuntime.InvokeAsync<string>(GoogleAnalyticsInterop.Navigate, TrackingId, uri).ConfigureAwait(false);

        LogDebugMessage($"[GTAG][{TrackingId}] Navigated: '{uri}'");
    }


    private void LogDebugMessage(string message)
    {
        _logger.LogDebug(message);
    }
}
