import { FormControl } from '@angular/forms';
import { MapType } from '../../../enums/maptype.enum';

export function targetValidator(typeGetter: () => MapType) {
    return (c: FormControl) => {
        const type = typeGetter();
        if (type == MapType.Gangwar && !c.value) {
            return { targetRequired: true };
        }
        return null;
    };
}
