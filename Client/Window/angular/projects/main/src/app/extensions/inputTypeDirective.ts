import { Directive, OnInit, HostListener } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../enums/dtoclientevent.enum';
import { SettingsService } from '../services/settings.service';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: "input"
})
export class InputTypeDirective {


    constructor(private rageConnector: RageConnectorService, private settings: SettingsService) {}

    @HostListener('focus', ['$event'])
    onFocus(e: any) {
        this.settings.InputFocused = true;
        this.rageConnector.call(DToClientEvent.InputStarted);
    }

    @HostListener('focusout', ['$event'])
    onFocusOut(e: any) {
        this.settings.InputFocused = false;
        this.rageConnector.call(DToClientEvent.InputStopped);
    }
}
