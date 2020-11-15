import { ValidatorFn, AbstractControl } from '@angular/forms';

export function maxNumberValidator(max: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any} | null => {
        if (!control.value || !control.value.length) {
            return null;
        }

        const value = Number(control.value);
        if (isNaN(value)) {
            return { max: "NaN" };
        }

        return value > max ? { max: "TooBig" } : null;
    };
}
