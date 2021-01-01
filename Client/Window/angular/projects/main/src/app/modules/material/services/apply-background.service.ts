import { Injectable, ElementRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { SettingsThemeIndex } from '../../../components/userpanel/userpanel-settings-normal/enums/settings-theme-index.enum';
import { EventEmitter } from 'events';

@Injectable()
export class ApplyBackgroundService {
    changed = new EventEmitter();

    private elements: any[] = [];
    private lastUseDarkTheme: boolean;
    private lastDarkBackgroundColor: string;
    private lastLightBackgroundColor: string;

    constructor(private settings: SettingsService) {
        this.settings.ThemeSettingChanged.on(null, this.revertAllBackgroundColorStyles.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.themeChanged.bind(this));
        this.settings.SettingsLoaded.on(null, this.themeSettingsLoaded.bind(this));
        this.themeSettingsLoaded();

        this.changed.setMaxListeners(50);
    }

    add(element: any) {
        this.elements.push(element);
        this.applyThemeOnElement(element);
    }

    remove(element: any) {
        const index = this.elements.indexOf(element);
        if (index > 0) {
            this.elements.splice(index, 1);
        }
    }

    private applyThemeOnElement(element: any) {
        element.style.backgroundColor = '';
        const colorStr = this.lastUseDarkTheme ? this.lastDarkBackgroundColor : this.lastLightBackgroundColor;
        element.style.background = colorStr;
    }

    private revertAllBackgroundColorStyles() {
        for (const element of this.elements) {
            element.style.backgroundColor = '';
        }
        this.changed.emit(null);
    }

    private themeChanged(key?: SettingsThemeIndex, value?: any) {
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
        for (const element of this.elements) {
            element.style.background = colorStr;
        }

        this.changed.emit(null);
    }

    private themeSettingsLoaded() {
        this.revertAllBackgroundColorStyles();
        this.lastUseDarkTheme = this.settings.Settings[13];
        this.lastDarkBackgroundColor = this.settings.Settings[17];
        this.lastLightBackgroundColor = this.settings.Settings[18];
        this.themeChanged();
    }
}
