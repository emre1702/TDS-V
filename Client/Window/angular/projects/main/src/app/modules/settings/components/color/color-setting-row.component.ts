import { Input, Component, ChangeDetectorRef, Output, EventEmitter } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { ColorSettingRow } from '../../models/color-setting-row';
import { SafeStyle, DomSanitizer } from '@angular/platform-browser';

@Component({
    selector: 'app-color-setting-row',
    templateUrl: './color-setting-row.component.html',
    styleUrls: ['./color-setting-row.component.scss']
})
export class ColorSettingRowComponent {
    @Input() setting: ColorSettingRow;

    @Output() changed = new EventEmitter();

    constructor(public settingsService: SettingsService, private sanitizer: DomSanitizer, private changeDetector: ChangeDetectorRef) {}

    onChange() {
        let value = this.setting.formControl.value as string;
        if (value.endsWith(";")) {
            value = value.substr(0, value.length - 1);
            this.setting.formControl.setValue(value);
            this.changeDetector.detectChanges();
        }

        if (this.setting.onValueChanged) {
            this.setting.onValueChanged(this.setting); 
        }
        this.changed.emit(null);
    }

    onColorChange(color: string) {
        if (color.endsWith(";")) {
            color = color.substr(0, color.length - 1);
        }
        this.setting.formControl.setValue(color);
        this.changeDetector.detectChanges();

        if (this.setting.onValueChanged) {
            this.setting.onValueChanged(this.setting); 
        }
        this.changed.emit(null);
    }

    deleteColor() {
        this.setting.formControl.setValue(undefined); 
        this.changeDetector.detectChanges()
    }

    getColor(color: string): SafeStyle {
        return this.sanitizer.bypassSecurityTrustStyle(color);
    }
}
