import { Directive, OnInit, HostListener } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from '../enums/to-client-event.enum';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: 'textarea',
})
export class TextareaTypeDirective {
    constructor(private rageConnector: RageConnectorService) {}

    @HostListener('focus', ['$event'])
    onFocus(e: any) {
        this.rageConnector.call(ToClientEvent.InputStarted);
    }

    @HostListener('focusout', ['$event'])
    onFocusOut(e: any) {
        this.rageConnector.call(ToClientEvent.InputStopped);
    }
}
