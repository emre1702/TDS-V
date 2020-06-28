import { Injectable, OnDestroy } from '@angular/core';
import { FormControl } from '@angular/forms';
import { LanguagePipe } from '../pipes/language.pipe';
import { SettingsService } from './settings.service';

export class FormControlCheck {
    public name: string;
    public formControl: FormControl;
}

export class CustomErrorCheck {
    public name: string;
    public checkValid: () => boolean;
    public errorKey: string;
}

@Injectable()
export class ErrorService implements OnDestroy {

    public errorMessage: string;

    private toCheck: (FormControlCheck | CustomErrorCheck)[];
    private langPipe = new LanguagePipe();
    private bindedLanguageChanged = false;

    constructor(private settings: SettingsService) {
        this.bindedLanguageChanged = true;
        this.settings.LanguageChanged.on(null, this.triggerCheck.bind(this));
    }

    ngOnDestroy() {
        if (this.bindedLanguageChanged) {
            this.bindedLanguageChanged = false;
            this.settings.LanguageChanged.off(null, this.triggerCheck.bind(this));
        }
    }

    public add(check: FormControlCheck | CustomErrorCheck) {
        this.toCheck.push(check);
    }

    public triggerCheck() {
        this.errorMessage = undefined;
        let errorData: { name: string, errorKey: string, data?: any };

        for (const check of this.toCheck) {

            if (check instanceof FormControlCheck) {
                if (!check.formControl.invalid) {
                    continue;
                }
                check.formControl.markAllAsTouched();
                const key = Object.keys(check.formControl.errors)[0];
                errorData = {
                    name: check.name,
                    errorKey: "Error" + key,
                    data: check.formControl.errors[key]
                };
                break;

            } else if (check instanceof CustomErrorCheck) {
                if (check.checkValid()) {
                    continue;
                }
                errorData = { name: check.name, errorKey: check.errorKey };
                break;
            }
        }

        if (errorData) {
            this.errorMessage = this.createErrorMessage(errorData);
        }
    }

    public hasError(): boolean {
        this.triggerCheck();
        return !!this.errorMessage;
    }

    /**
     * Toggle if changed language should trigger the check.
     * @param toggle Default: true
     */
    public setTriggerCheckOnLanguageChange(toggle: boolean) {
        if (this.bindedLanguageChanged == toggle) {
            return;
        }
        this.bindedLanguageChanged = toggle;
        if (toggle) {
            this.settings.LanguageChanged.on(null, this.triggerCheck.bind(this));
        } else {
            this.settings.LanguageChanged.off(null, this.triggerCheck.bind(this));
        }
    }

    private createErrorMessage(data: { name: string, errorKey: string, data?: any }): string {
        let msg = "[" + data.name + "] "
                + this.langPipe.transform(data.errorKey, this.settings.Lang);
        if (!data.data) {
            return msg;
        }

        let info: string;

        switch (typeof data.data) {
            case "object":
                console.log(typeof data.data);
                console.log(data.data);
                break;
            case "undefined":
            case "symbol":
            case "function":
                console.log(typeof data.data);
                console.log(data.data);
                break;

            default:
                info = String(data.data);
        }

        if (info && info.length) {
            msg += this.langPipe.transform("Info: ", this.settings.Lang) + this.langPipe.transform(info, this.settings.Lang);
        }

        return msg;
    }
}
