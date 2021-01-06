import { FormControl } from '@angular/forms';

export function valueMoreThanOrEqualValidator(otherC: FormControl) {
    return (c: FormControl) => {
        if (!c.value || !otherC.value) return null;

        if (c.value >= otherC.value) return null;

        return { valueMoreThanOrEqual: { expected: otherC.value, actual: c.value } };
    };
}
