using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace GoogleAnalytics.Blazor;


/// <summary>
/// Google Analytics event and navigation tracking.
/// </summary>
public interface IGBAnalyticsManager
{
    /// <summary>
    /// Disable global tracking.
    /// </summary>
    public void DisableGlobalTracking();


    /// <summary>
    /// Enable global tracking.
    /// </summary>
    public void EnableGlobalTracking();


    /// <summary>
    /// Gets additional config info. See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/>.
    /// </summary>
    /// <param name="additionalConfigInfo"></param>
    /// <returns></returns>
    public ImmutableDictionary<string, object> GetAdditionalConfigInfo();


    /// <summary>
    /// Gets event parameters to be used for every event tracked. Any event tracked can add further event parameters.
    /// See <see href="https://developers.google.com/tag-platform/gtagjs/reference#event"/>.
    /// </summary>
    /// <returns></returns>
    public ImmutableDictionary<string, object> GetGlobalEventParams();


    /// <summary>
    /// Gets the tracking id.
    /// </summary>
    /// <returns>The current tracking id</returns>
    public string GetTrackingId();


    /// <summary>
    /// True if global tracking is enabled.
    /// </summary>
    public bool IsGlobalTrackingEnabled();


    /// <summary>
    /// Sets additional config info. See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/>.
    /// </summary>
    /// <param name="additionalConfigInfo"></param>
    /// <returns></returns>
    public Task SetAdditionalConfigInfo(IDictionary<string, object> additionalConfigInfo);


    /// <summary>
    /// Sets event parameters to be used for every event tracked. Any event tracked can add further event parameters.
    /// See <see href="https://developers.google.com/tag-platform/gtagjs/reference#event"/>.
    /// </summary>
    /// <param name="globalEventParams"></param>
    /// <returns></returns>
    public void SetGlobalEventParams(IDictionary<string, object> globalEventParams);


    /// <summary>
    /// Sets the tracking id.
    /// </summary>
    /// <param name="trackingId"></param>
    /// <returns></returns>
    public Task SetTrackingId(string trackingId);


    /// <summary>
    /// Suppresses tracking notification to GA of a page hit. Place a call
    /// to this function in <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnInitialized"/>
    /// or <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnInitializedAsync"/> to suppress
    /// that page's tracking.
    /// </summary>
    public void SuppressNextPageHitTracking();
    /// <summary>
    /// Tracks an event to Google Analytics. See <see href="https://developers.google.com/analytics/devguides/collection/ga4/reference/events"/> for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <param name="eventValue"></param>
    /// <returns></returns>

    public Task TrackEvent(string eventName, string eventCategory = null, string eventLabel = null, int? eventValue = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See <see href="https://developers.google.com/analytics/devguides/collection/ga4/reference/events"/> for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventValue"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <returns></returns>
    public Task TrackEvent(string eventName, int eventValue, string eventCategory = null, string eventLabel = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See <see href="https://developers.google.com/analytics/devguides/collection/ga4/reference/events"/> for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    /// <returns></returns>
    public Task TrackEvent(string eventName, object eventData);

}
