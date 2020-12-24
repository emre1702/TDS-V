import { Injectable, OnDestroy } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { SettingsService } from '../../../services/settings.service';
import { LanguagePipe } from '../pipes/language.pipe';

export class FormControlCheck {
    public readonly type = 'FormControlCheck';

    public name: string;
    public formControl: AbstractControl;

    constructor(name: string, formControl: AbstractControl) {
        this.name = name;
        this.formControl = formControl;
    }
}

export class CustomErrorCheck {
    public readonly type = 'CustomErrorCheck';

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

    add(check: FormControlCheck | CustomErrorCheck) {
        this.toCheck.push(check);
    }

    triggerCheck() {
        this.errorMessage = undefined;
        let errorData: { name: string; errorKey: string; data?: any };

        for (const check of this.toCheck) {
            if (check.type === 'FormControlCheck') {
                const formControlCheck = check as FormControlCheck;
                if (!formControlCheck.formControl.invalid) {
                    continue;
                }
                formControlCheck.formControl.markAllAsTouched();
                const key = Object.keys(formControlCheck.formControl.errors)[0];
                errorData = {
                    name: check.name,
                    errorKey: 'Error' + key,
                    data: formControlCheck.formControl.errors[key],
                };
                break;
            } else if (check.type === 'CustomErrorCheck') {
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

    private createErrorMessage(data: { name: string; errorKey: string; data?: any }): string {
        let msg = '[' + this.langPipe.transform(data.name, this.settings.Lang) + '] ' + this.langPipe.transform(data.errorKey, this.settings.Lang);
        if (!data.data) {
            return msg;
        }

        let info: string;

        switch (typeof data.data) {
            case 'object':
                switch (data.errorKey) {
                    case 'Errorminlength':
                        info = this.getMinLengthInfo(data.data);
                        break;
                    case 'Errormaxlength':
                        info = this.getMaxLengthInfo(data.data);
                        break;
                    default:
                        console.log(typeof data.data);
                        console.log(data);
                        break;
                }

                break;
            case 'undefined':
            case 'symbol':
            case 'function':
                console.log(typeof data.data);
                console.log(data);
                break;

            case 'boolean':
                break;

            default:
                info = String(data.data);
        }

        if (info?.length) {
            msg += ' Info: ' + this.langPipe.transform(info, this.settings.Lang);
        }

        return msg;
    }

    private getMinLengthInfo(data: { requiredLength: number; actualLength: number }) {
        return `${this.langPipe.transform('Min', this.settings.Lang)}: ${data.requiredLength} | ${this.langPipe.transform('Actual', this.settings.Lang)}: ${
            data.actualLength
        }`;
    }

    private getMaxLengthInfo(data: { requiredLength: number; actualLength: number }) {
        return `${this.langPipe.transform('Max', this.settings.Lang)}: ${data.requiredLength} | ${this.langPipe.transform('Actual', this.settings.Lang)}: ${
            data.actualLength
        }`;
    }
}
