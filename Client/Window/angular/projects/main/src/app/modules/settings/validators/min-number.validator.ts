import { ValidatorFn, AbstractControl } from '@angular/forms';

export function minNumberValidator(min: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any} | null => {
        if (!control.value || !control.value.length) {
            return null;
        }

        const value = Number(control.value);
        if (isNaN(value)) {
            return { min: "NaN" };
        }

        return value < min ? { min: "TooSmall" } : null;
    };
}
