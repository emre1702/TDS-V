import { FormControl } from '@angular/forms';

export function valueLessThanOrEqualValidator(otherC: FormControl) {
    return (c: FormControl) => {
        if (!c.value || !otherC.value) return null;

        if (c.value <= otherC.value) return null;

        return { valueLessThanOrEqual: { expected: otherC.value, actual: c.value } };
    };
}
