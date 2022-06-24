using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace GoogleAnalytics.Blazor;

/// <summary>
/// Anchor component for GoogleAnalytics.Blazor to be placed in App.razor.
/// </summary>
public class GBAnchor : ComponentBase
{
    [Inject] private IGBAnalyticsManager GBAnalyticsManager { get; set; }


    /// <inheritdoc/>
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await (GBAnalyticsManager as GBAnalyticsManager).Initialize().ConfigureAwait(false);
        }
    }
}
