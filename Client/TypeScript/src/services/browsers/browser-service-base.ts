import alt from "alt-client";
import ToBrowserEvent from "../../datas/enums/events/to-browser-event.enum";

export default class BrowserServiceBase {
    protected browser: alt.WebView;
    private hasLoaded = false;
    private url: string;
    private executeList: (() => void)[] = [];


    constructor(url: string) {
        this.url = url;
    }

    createBrowser(isOverlay: boolean = false) {
        if (this.browser) {
            this.browser.destroy();
        }

        this.browser = new alt.WebView(this.url, isOverlay);
        this.browser.on("load", this.browserLoaded.bind(this));
    }

    execute(eventName: ToBrowserEvent | string, ...args: any[]) {
        if (!this.browser || !this.hasLoaded) {
            this.executeList.push(() => this.browser.emit(eventName, args));
        } else {
            this.browser.emit(eventName, args);
        }
    }

    setReady(...args: any[]) {
        this.execute(ToBrowserEvent.InitLoadAngular, args);
        this.browser.focus();
    }

    stop() {
        if (!this.browser) {
            return;
        }
        this.browser.destroy();
        this.browser = undefined;
        this.executeList.length = 0;
    }

    private browserLoaded() {
        if (!this.browser) {
            return;
        }
        this.hasLoaded = true;
        for (const exec of this.executeList)
        {
            exec();
        }
        this.executeList.length = 0;
    }
}
