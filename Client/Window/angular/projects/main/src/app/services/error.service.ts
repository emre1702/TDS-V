import { Injectable, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { FormControl } from '@angular/forms';
import { SettingsService } from './settings.service';
import { LanguagePipe } from '../modules/shared/pipes/language.pipe';

export class FormControlCheck {
    public readonly type = "FormControlCheck";

    public name: string;
    public formControl: FormControl;

    constructor(name: string, formControl: FormControl) {
        this.name = name;
        this.formControl = formControl;
    }
}

export class CustomErrorCheck {
    public readonly type = "CustomErrorCheck";

    public name: string;
    public checkValid: () => boolean;
    public errorKey: string;

    constructor(name: string, checkValid: () => boolean, errorKey: string) {
        this.name = name;
        this.checkValid = checkValid;
        this.errorKey = errorKey;
    }
}

@Injectable()
export class ErrorService implements OnDestroy {

    public errorMessage: string;

    private toCheck: (FormControlCheck | CustomErrorCheck)[] = [];
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

            if (check.type === "FormControlCheck") {
                const formControlCheck = check as FormControlCheck;
                if (!formControlCheck.formControl.invalid) {
                    continue;
                }
                formControlCheck.formControl.markAllAsTouched();
                const key = Object.keys(formControlCheck.formControl.errors)[0];
                errorData = {
                    name: check.name,
                    errorKey: "Error" + key,
                    data: formControlCheck.formControl.errors[key]
                };
                break;

            } else if (check.type === "CustomErrorCheck") {
                const customErrorCheck = check as CustomErrorCheck;
                if (customErrorCheck.checkValid()) {
                    continue;
                }
                errorData = { name: check.name, errorKey: customErrorCheck.errorKey };
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
