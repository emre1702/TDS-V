import { Directive, ViewContainerRef } from '@angular/core';
import { MatSelect } from '@angular/material/select';

@Directive({
    selector: 'mat-select',
})
export class FocusRemoverDirective {
    constructor(viewContainerRef: ViewContainerRef) {
        const matSelect = (viewContainerRef as any)._data?.componentView?.component as MatSelect;
        if (!matSelect) return;
        matSelect.selectionChange.subscribe(() => matSelect.close());
    }
}
