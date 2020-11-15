import { Directive, OnInit, OnDestroy, ChangeDetectorRef, ViewContainerRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { SettingsThemeIndex } from '../../../components/userpanel/userpanel-settings-normal/enums/settings-theme-index.enum';

@Directive({
  selector: ".mat-app-background, .mat-menu-panel",
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
        this.settings.SettingsLoaded.on(null, this.themeSettingsLoaded.bind(this));
        this.themeSettingsLoaded();
    }

    ngOnDestroy() {
        this.settings.ThemeSettingChanged.off(null, this.revertBackgroundColorStyle.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.themeChanged.bind(this));
        this.settings.SettingsLoaded.off(null, this.themeSettingsLoaded.bind(this));
    }

    private revertBackgroundColorStyle() {
        this.viewContainerRef.element.nativeElement.style.backgroundColor = "";
        this.changeDetector.detectChanges();
    }

    themeChanged(key?: SettingsThemeIndex, value?: any) {
        if (key != undefined) {
            switch (key) {
                case SettingsThemeIndex.UseDarkTheme:
                    this.lastUseDarkTheme = value;
                    break;
                case SettingsThemeIndex.ThemeBackgroundDarkColor:
                    this.lastDarkBackgroundColor = value;
                    break;
                case SettingsThemeIndex.ThemeBackgroundLightColor:
                    this.lastLightBackgroundColor = value;
                    break;
            }
        }

        const colorStr = this.lastUseDarkTheme ? this.lastDarkBackgroundColor : this.lastLightBackgroundColor;
        this.viewContainerRef.element.nativeElement.style.background = colorStr;
        this.changeDetector.detectChanges();
    }

    themeSettingsLoaded() {
        this.revertBackgroundColorStyle();
        this.lastUseDarkTheme = this.settings.Settings[13];
        this.lastDarkBackgroundColor = this.settings.Settings[17];
        this.lastLightBackgroundColor = this.settings.Settings[18];
        this.themeChanged();
    }
}
