import { Directive, OnInit, OnDestroy, ChangeDetectorRef, ViewContainerRef } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { UserpanelSettingKey } from '../components/userpanel/enums/userpanel-setting-key.enum';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: ".mat-app-background"
})
export class MatAppBackgroundDirective implements OnInit, OnDestroy {
    private lastUseDarkTheme: boolean;
    private lastDarkBackgroundColor: string;
    private lastLightBackgroundColor: string;

    constructor(
        private settings: SettingsService,
        private changeDetector: ChangeDetectorRef,
        private viewContainerRef: ViewContainerRef) {

    }

    ngOnInit() {
        this.settings.ThemeSettingChanged.on(null, this.revertBackgroundColorStyle.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.themeChanged.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.themeSettingsLoaded.bind(this));
        this.themeSettingsLoaded();
    }

    ngOnDestroy() {
        this.settings.ThemeSettingChanged.off(null, this.revertBackgroundColorStyle.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.themeChanged.bind(this));
        this.settings.ThemeSettingsLoaded.off(null, this.themeSettingsLoaded.bind(this));
    }

    private revertBackgroundColorStyle() {
        this.viewContainerRef.element.nativeElement.style.backgroundColor = "";
        this.changeDetector.detectChanges();
    }

    private getCurrentColor() {
        const currentColor = window.getComputedStyle(this.viewContainerRef.element.nativeElement).backgroundColor;
        if (currentColor === "") {
            return;
        }
        return this.getRGBAFromColorString(currentColor);
    }

    private getRGBAFromColorString(str: string): { r: number, g: number, b: number, a: number } {
        let rgba: { r: number, g: number, b: number, a: number };
        if (str.indexOf("rgb") >= 0) {
            const result = str.match(/^rgba?[\s+]?\([\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?,[\s+]?(\d+)[\s+]?/i);
            rgba = {
                r: parseInt(result[1], 10),
                g: parseInt(result[2], 10),
                b: parseInt(result[3], 10),
                a: this.settings.ThemeSettings[1] / 100
            };
        } else {
            const result = str.match(/^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i);
            rgba = {
                r: parseInt(result[1], 16),
                g: parseInt(result[2], 16),
                b: parseInt(result[3], 16),
                a: this.settings.ThemeSettings[1] / 100
            };
        }
        return rgba;
    }

    themeChanged(key?: UserpanelSettingKey, value?: any) {
        switch (key) {
            case UserpanelSettingKey.ThemeBackgroundDarkColor:
                this.lastDarkBackgroundColor = value;
                break;
            case UserpanelSettingKey.ThemeBackgroundLightColor:
                this.lastLightBackgroundColor = value;
                break;
            case UserpanelSettingKey.UseDarkTheme:
                this.lastUseDarkTheme = value;
                break;
        }

        const colorStr = this.lastUseDarkTheme ? this.lastDarkBackgroundColor : this.lastLightBackgroundColor;
        try {
            const rgba = this.getRGBAFromColorString(colorStr);
            this.viewContainerRef.element.nativeElement.style.background = `rgba(${rgba.r}, ${rgba.g}, ${rgba.b}, ${rgba.a})`;
        } catch {
            this.viewContainerRef.element.nativeElement.style.background = colorStr;
        }
        this.changeDetector.detectChanges();
    }

    themeSettingsLoaded() {
        this.revertBackgroundColorStyle();
        this.lastUseDarkTheme = this.settings.ThemeSettings[0];
        this.lastDarkBackgroundColor = this.settings.ThemeSettings[5];
        this.lastLightBackgroundColor = this.settings.ThemeSettings[6];
        this.themeChanged();
    }
}
