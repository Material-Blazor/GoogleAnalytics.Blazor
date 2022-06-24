# GoogleAnalytics.Blazor

## Blazor extensions for Google Analytics

Forked from [Blazor-Analytics](https://github.com/isc30/blazor-analytics), itself MIT License, Copyright (c) 2019 Ivan Sanz Carasa.

Developed by [Material-Blazor](https://material-blazor.com).

---


[![NuGet version](https://img.shields.io/nuget/v/GoogleAnalytics.Blazor?logo=nuget&label=nuget%20version&style=flat-square)](https://www.nuget.org/packages/GoogleAnalytics.Blazor/)
[![NuGet version](https://img.shields.io/nuget/vpre/GoogleAnalytics.Blazor?logo=nuget&label=nuget%20pre-release&style=flat-square)](https://www.nuget.org/packages/GoogleAnalytics.Blazor/)
[![NuGet downloads](https://img.shields.io/nuget/dt/GoogleAnalytics.Blazor?logo=nuget&label=nuget%20downloads&style=flat-square)](https://www.nuget.org/packages/GoogleAnalytics.Blazor/)


---


[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?logo=github&style=flat-square)](/LICENSE.md)
[![GitHub issues](https://img.shields.io/github/issues/Material-Blazor/GoogleAnalytics.Blazor?logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/issues)
[![GitHub forks](https://img.shields.io/github/forks/Material-Blazor/GoogleAnalytics.Blazor?logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/network/members)
[![GitHub stars](https://img.shields.io/github/stars/Material-Blazor/GoogleAnalytics.Blazor?logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/stargazers)
[![GitHub stars](https://img.shields.io/github/watchers/Material-Blazor/GoogleAnalytics.Blazor?logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/watchers)

[![GithubActionsMainPublish](https://img.shields.io/github/workflow/status/Material-Blazor/GoogleAnalytics.Blazor/GithubActionsRelease?label=actions%20release&logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/actions?query=workflow%3AGithubActionsRelease)
[![GithubActionsDevelop](https://img.shields.io/github/workflow/status/Material-Blazor/GoogleAnalytics.Blazor/GithubActionsWIP?label=actions%20wip&logo=github&style=flat-square)](https://github.com/Material-Blazor/GoogleAnalytics.Blazor/actions?query=workflow%3AGithubActionsWIP)


# Configuration

## All Projects

First, import the namespaces in `_Imports.razor`

```csharp
+@using GoogleAnalytics.Blazor
```

Then, add the `GBAnchor` component to the end of `App.razor`.

```csharp diff
+   <NavigationTracker />
```

## ServerSide Specific Configuration

Edit `_Layout.cshtml`:

```html diff
    <script src="_framework/blazor.server.js"></script>
+   <script src="_content/GoogleAnalytics.Blazor/googleanalytics.blazor.js"></script>
```

## WebAssembly Specific Configuration

Edit `index.html` and apply the following change:

```html diff
    <script src="_framework/blazor.webassembly.js"></script>
+   <script src="_content/GoogleAnalytics.Blazor/googleanalytics.blazor.js"></script>
```

## Setting up GoogleAnalytics

Inside `Program.cs`, call `AddGBService`. This will configure `[YOUR_GTAG_ID]` automatically. You can elect to 
omit the tag id and call `SetTargetId` from the service injected into a component or service at a later stage; this
is helpful if for instance you need to run a back end query in order to determine the tag.

```csharp diff
+   builder.Services.AddGBService("[YOUR_GTAG_ID]");
```

You may wish to set [additional config info](https://developers.google.com/tag-platform/gtagjs/reference#config) and/or
globally applicable [event params](https://developers.google.com/tag-platform/gtagjs/reference#event) (i.e. applied to every call to `TrackEvent`):

```csharp diff
+   builder.Services.AddGBService(
+       "[YOUR_GTAG_ID]",
+       additionalConfigInfo: new Dictionary<string, object>()
+       {
+           { "user_id", "[YOUR_USER_ID]" }
+       },
+       globalEventParams: new Dictionary<string, object>()
+       {
+           { "user_id", "YOUR_USER_ID" }
+       });
```

You can set all three of these items using `IGBAnalyticsManager.SetTrackingId()`, `IGBAnalyticsManager.SetAdditionalConfigInfo()` and `IGBAnalyticsManager.SetGlobalEventParams()`
respectively, after having injected the `IGBAnalyticsManager` service into a component or another service (either scoped or transient).

# How to trigger an Analytics Event

1. Inject `IGBAnalyticsManager` wherever you want to trigger the event.
2. Call `IGBAnalyticsManager.TrackEvent` passing the `EventName` and `EventData` (an object containing the event data).


Or


Call `IGBAnalyticsManager.TrackEvent` passing the `EventName`, `Value` and `Category` (optional).

```csharp
@using GoogleAnalytics.Blazor

@inject IGBAnalyticsManager AnalyticsManager

AnalyticsManager.TrackEvent("generate_lead", new {currency = "USD", value = 99.99});
```

# How to disable tracking on any page

1. Inject `IGBAnalyticsManager` into a blazor component.

```csharp
@using GoogleAnalytics.Blazor

@inject IGBAnalyticsManager AnalyticsManager
```

2. Suppress tracking for just the current page.

```csharp
protected override void OnInitialized()
{
    AnalyticsManager.SuppressNextPageHitTracking();
}
```

3. Disable page tracking for the whole application.

```csharp
protected override void OnInitialized()
{
    AnalyticsManager.DisableGlobalTracking();
}
```

4. Re-enable page tracking for the whole application.

```csharp
protected override void OnInitialized()
{
    AnalyticsManager.EnableGlobalTracking();
}
```
  
