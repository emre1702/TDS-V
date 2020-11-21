import { Component, Input, Output, EventEmitter, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingRow } from './models/setting-row';
import { SettingType } from '../../enums/setting-type';
import { SettingsService } from '../../services/settings.service';
import { FormGroup } from '@angular/forms';
import { SettingChangedEvent } from './interfaces/setting-changed-event';

@Component({
    selector: 'app-settings',
    templateUrl: './settings.component.html',
    styleUrls: ['./settings.component.scss']
})
export class SettingsComponent implements OnDestroy {
    
    private _settings: SettingRow[];
    get settings(): SettingRow[] {
        return this._settings;
    }
    @Input() set settings(value: SettingRow[]) {
        this._settings = value;
        this.setValuesInFormControls();
        this.createFormGroup();
        this.storeOldSettings();
        this.changeDetector.detectChanges();
    }
    @Input() canSave: boolean = true;
    @Input() canRevertAll: boolean;
    @Input() canSetDefault: boolean;

    @Output() saved = new EventEmitter<{}>();
    @Output() changed = new EventEmitter<SettingChangedEvent>();

    formGroup: FormGroup;
    settingType = SettingType;

    private oldValues: { [key: number]: any } = {};
    private hasBeenSaved: boolean;

    constructor(public settingsService: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnDestroy() {
        if (!this.hasBeenSaved) {
            this.restoreOldSettings();
        }
    }

    save() {
        this.hasBeenSaved = true;
        const settings = this.settings.filter(s => s.formControl);
        for (const setting of settings) {
            setting.containerGetter()[setting.dataSettingIndex] = setting.formControl.value;
        }
        this.saved.emit(this.formGroup.value);
    }

    revertAll() {
        const settings = this.settings.filter(s => s.formControl);
        for (const setting of settings) {
            const currentValue = setting.formControl.value;
            setting.formControl.setValue(this.oldValues[setting.dataSettingIndex], 
                { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
            setting.containerGetter()[setting.dataSettingIndex] = setting.formControl.value;

            if (currentValue != setting.formControl.value) {
                this.changed.emit({ index: setting.dataSettingIndex, value: setting.formControl.value });
                if (setting.onValueChanged) {
                    setting.onValueChanged(setting);
                }
            }
        }
        this.changeDetector.detectChanges();
    }

    setDefault() {
        const settings = this.settings.filter(s => s.formControl);
        for (const setting of settings) {
            const currentValue = setting.formControl.value;
            setting.formControl.setValue(setting.defaultValue, { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
            setting.containerGetter()[setting.dataSettingIndex] = setting.formControl.value;

            if (currentValue != setting.formControl.value) {
                this.changed.emit({ index: setting.dataSettingIndex, value: setting.formControl.value });
                if (setting.onValueChanged) {
                    setting.onValueChanged(setting);
                }
            }
        }
        this.changeDetector.detectChanges();
    }

    onChanged(index: number, value: any) {
        const data = {
            index: index,
            value: value
        }
        this.changed.emit(data);
    }

    private setValuesInFormControls() {
        const settings = this.settings.filter(s => s.formControl);
        for (const setting of settings) {
            setting.formControl.setValue(setting.containerGetter()[setting.dataSettingIndex], 
            { emitEvent: true, emitModelToViewChange: true, emitViewToModelChange: true });
        }
    }

    private createFormGroup() {
        const settings = this.settings.filter(s => s.formControl);
        const formControls = {};
        for (const setting of settings) {
            formControls[String(setting.dataSettingIndex)] = setting.formControl;
        }
        this.formGroup = new FormGroup(formControls);
        this.formGroup.markAllAsTouched();
        this.changeDetector.detectChanges();
    }

    private storeOldSettings() {
        const settings = this.settings.filter(s => s.formControl);
        this.oldValues = {};
        for (const setting of settings) {
            this.oldValues[setting.dataSettingIndex] = setting.formControl.value;
        }
    }

    private restoreOldSettings() {
        const settings = this.settings.filter(s => s.formControl);
        for (const setting of settings) {
            setting.containerGetter()[setting.dataSettingIndex] = setting.formControl.value;
        }
    }
}
