import { Directive, HostListener } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../enums/dtoclientevent.enum';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: "input"
})
export class InputTypeDirective {

    constructor(private rageConnector: RageConnectorService) {}

    @HostListener('focus', ['$event'])
    onFocus(e: any) {
        this.rageConnector.call(DToClientEvent.InputStarted);
    }

    @HostListener('focusout', ['$event'])
    onFocusOut(e: any) {
        this.rageConnector.call(DToClientEvent.InputStopped);
    }
}
