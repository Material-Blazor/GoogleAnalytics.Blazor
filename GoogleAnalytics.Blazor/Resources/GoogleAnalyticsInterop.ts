// declare window globals
interface Window
{
    dataLayer: any[];
    gtag: (...args: any[]) => void;
}

interface ObjectConstructor {
    assign(...objects: Object[]): Object;
}

interface AdditionalConfigInfoObject {
    [key: string]: any
}

interface EventParamsObject {
    [key: string]: any
}


// declare globals
declare const dataLayer: any[];
declare const gtag: (...args: any[]) => void;

// init globals
window.dataLayer = window.dataLayer || [];
window.gtag = window.gtag || function () { dataLayer.push(arguments); };

// configure first timestamp
gtag("js", new Date());

namespace GoogleAnalyticsInterop
{
    export function configure(trackingId: string, additionalConfigInfo: AdditionalConfigInfoObject): void
    {
        this.additionalConfigInfo = additionalConfigInfo;
        const script = document.createElement("script");
        script.async = true;
        script.src = "https://www.googletagmanager.com/gtag/js?id=" + trackingId;

        document.head.appendChild(script);

        let configObject: AdditionalConfigInfoObject = {};
        configObject.send_page_view = false;
        Object.assign(configObject, additionalConfigInfo)

        gtag("config", trackingId, configObject);
    }

    export function navigate(trackingId: string, href: string): void
    {
        let configObject: AdditionalConfigInfoObject = {};

        configObject.page_location = href;
        Object.assign(configObject, this.additionalConfigInfo)
        gtag("config", trackingId, configObject);
    }

    export function trackEvent(eventName: string, eventParams: EventParamsObject, globalEventParams: EventParamsObject)
    {
        Object.assign(eventParams, globalEventParams)

        gtag("event", eventName, eventParams);
    }
}
