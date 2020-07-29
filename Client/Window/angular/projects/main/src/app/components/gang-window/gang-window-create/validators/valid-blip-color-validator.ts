import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { Constants } from 'projects/main/src/app/constants';

export function validBlipColorValidator()
    : ValidatorFn {
    return (control: AbstractControl): ValidationErrors => {
        const blipId = control.value;
        if (!Constants.BLIP_COLORS.find(c => c.ID == blipId)) {
            return { ["blipColorDoesntExists"]: true };
        }
        return null;
    };
}

