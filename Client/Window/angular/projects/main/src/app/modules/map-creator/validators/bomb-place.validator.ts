import { FormControl } from '@angular/forms';
import { MapType } from '../../../enums/maptype.enum';

export function bombPlaceValidator(typeGetter: () => MapType) {
    return (c: FormControl) => {
        const type = typeGetter();
        if (type == MapType.Bomb && !c.value?.length) {
            return { bombPlaceRequired: true };
        }
        return null;
    };
}
