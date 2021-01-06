import { FormControl } from '@angular/forms';
import { MapType } from 'projects/main/src/app/enums/maptype.enum';

export function mapLimitsValidator(typeGetter: () => MapType) {
    return (c: FormControl) => {
        const type = typeGetter();

        if (type == MapType.Gangwar && !c.value.length) {
            return { mapLimitRequired: true };
        }

        if (c.value.length && c.value.length < 3) {
            return { mapLimitInvalid: true };
        }

        return null;
    };
}
