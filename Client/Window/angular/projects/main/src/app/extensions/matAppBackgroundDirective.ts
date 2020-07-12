import { Directive, OnInit, OnDestroy, ChangeDetectorRef, ViewContainerRef } from '@angular/core';
import { SettingsService } from '../services/settings.service';
import { UserpanelSettingKey } from '../components/userpanel/enums/userpanel-setting-key.enum';
import { ThemeSettings } from '../interfaces/theme-settings';

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
        this.themeSettingsLoaded(this.settings.ThemeSettings);
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
        this.viewContainerRef.element.nativeElement.style.background = colorStr;
        this.changeDetector.detectChanges();
    }

    themeSettingsLoaded(settings: ThemeSettings) {
        this.revertBackgroundColorStyle();
        this.lastUseDarkTheme = settings[1000];
        this.lastDarkBackgroundColor = settings[1004];
        this.lastLightBackgroundColor = settings[1005];
        this.themeChanged();
    }
}
