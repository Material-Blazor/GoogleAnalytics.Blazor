using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleAnalytics.Blazor;


/// <summary>
/// Google Analytics event and navigation tracking.
/// </summary>
public interface IGBAnalyticsManager
{
    /// <summary>
    /// Sets the tracking id.
    /// </summary>
    /// <param name="trackingId"></param>
    /// <returns></returns>
    void SetTrackingId(string trackingId);


    /// <summary>
    /// Gets the tracking id.
    /// </summary>
    /// <returns>The current tracking id</returns>
    string GetTrackingId();


    /// <summary>
    /// Remove Obsolete attribute once functionality is determined.
    /// </summary>
    /// <param name="globalConfigData"></param>
    /// <returns></returns>
    [Obsolete]
    Task ConfigureGlobalConfigData(Dictionary<string, object> globalConfigData);


    /// <summary>
    /// Remove Obsolete attribute once functionality is determined.
    /// </summary>
    /// <param name="globalConfigData"></param>
    /// <returns></returns>
    [Obsolete]
    Task ConfigureGlobalEventData(Dictionary<string, object> globalEventData);


    /// <summary>
    /// Tracks navigation to a new endpoint.
    /// </summary>
    /// <param name="uri"></param>
    /// <returns></returns>
    Task TrackNavigation(string uri);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <param name="eventValue"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, string eventCategory = null, string eventLabel = null, int? eventValue = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventValue"></param>
    /// <param name="eventCategory"></param>
    /// <param name="eventLabel"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, int eventValue, string eventCategory = null, string eventLabel = null);


    /// <summary>
    /// Tracks an event to Google Analytics. See https://developers.google.com/analytics/devguides/collection/ga4/reference/events for a generic GA events.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventData"></param>
    /// <returns></returns>
    Task TrackEvent(string eventName, object eventData);


    /// <summary>
    /// Enable global tracking.
    /// </summary>
    void EnableGlobalTracking();


    /// <summary>
    /// Disable global tracking.
    /// </summary>
    void DisableGlobalTracking();


    /// <summary>
    /// True if global tracking is enabled.
    /// </summary>
    bool IsGlobalTrackingEnabled();


    /// <summary>
    /// Suppresses tracking notification to GA of a page hit. Place a call
    /// to this function in <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnInitialized"/>
    /// or <see cref="Microsoft.AspNetCore.Components.ComponentBase.OnInitializedAsync"/> to suppress
    /// that page's tracking.
    /// </summary>
    public void SuppressPageHitTracking();
}
