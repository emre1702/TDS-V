import { Directive, OnInit, HostListener } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { SettingsService } from '../../../services/settings.service';

@Directive({
    // tslint:disable-next-line: directive-selector
    selector: 'textarea',
})
export class TextareaTypeDirective {
    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {}

    @HostListener('focus', ['$event'])
    onFocus(e: any) {
        this.settings.InputFocused = true;
        this.rageConnector.call(ToClientEvent.InputStarted);
    }

    @HostListener('focusout', ['$event'])
    onFocusOut(e: any) {
        this.settings.InputFocused = false;
        this.rageConnector.call(ToClientEvent.InputStopped);
    }
}
