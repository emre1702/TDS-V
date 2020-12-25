import { Injectable } from '@angular/core';
import { EventEmitter } from 'events';

@Injectable({ providedIn: 'root' })
export class ApplyBackgroundService {
    changed = new EventEmitter();

    private elements: any[] = [];
    private lastUseDarkTheme: boolean;
    private lastDarkBackgroundColor: string;
    private lastLightBackgroundColor: string;

    constructor() {
        this.themeSettingsLoaded();
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

    private themeChanged() {
        const colorStr = this.lastUseDarkTheme ? this.lastDarkBackgroundColor : this.lastLightBackgroundColor;
        for (const element of this.elements) {
            element.style.background = colorStr;
        }

        this.changed.emit(null);
    }

    private themeSettingsLoaded() {
        this.revertAllBackgroundColorStyles();
        this.lastUseDarkTheme = true;
        this.lastDarkBackgroundColor = 'linear-gradient(0deg, rgba(2,0,36,0.87) 0%, rgba(23,52,111,0.87) 100%)';
        this.lastLightBackgroundColor = 'rgba(250, 250, 250, 0.87)';
        this.themeChanged();
    }
}
