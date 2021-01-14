import { Directive } from '@angular/core';
import { MatSelect } from '@angular/material/select';

@Directive({
    selector: 'mat-select',
})
export class MatSelectFocusRemoverDirective {
    constructor(matSelect: MatSelect) {
        if (!matSelect) return;
        matSelect.selectionChange.subscribe(() => {
            matSelect.close();
        });
    }
}
