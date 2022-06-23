using Microsoft.AspNetCore.Components;
using System.Diagnostics.CodeAnalysis;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Anchor component for GoogleAnalytics.Blazor to be placed in App.razor.
/// </summary>
public class GBAnchor : ComponentBase
{
    [SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Injected to ensure  service startup startup")]
    [Inject] private IGBAnalyticsManager GBAnalyticsManager { get; set; }
}
