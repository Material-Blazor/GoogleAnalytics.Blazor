using System.Collections.Generic;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Options for Google Analytics.
/// </summary>
public class GBOptions
{
    /// <summary>
    /// The Google Analytics tracking id.
    /// </summary>
    public string TrackingId { get; set; } = string.Empty;


    /// <summary>
    /// Additional configuration information. See <see href="https://developers.google.com/tag-platform/gtagjs/reference#config"/>.
    /// </summary>
    public IDictionary<string, object> AdditionalConfigInfo { get; set; } = default;


    /// <summary>
    /// Event parameters to be used for every event tracked. Any event tracked can add further event parameters.
    /// See <see href="https://developers.google.com/tag-platform/gtagjs/reference#event"/>.
    /// </summary>
    public IDictionary<string, object> GlobalEventParams { get; set; } = default;
}
