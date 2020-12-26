import { Component, Output, EventEmitter, Input, ChangeDetectorRef, OnInit, OnDestroy } from '@angular/core';
import { BodyMenuNav } from '../../enums/body-menu-nav.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { BodyGeneralData } from '../../models/body-general-data';
import { MatSelectChange } from '@angular/material/select';
import { MatRadioChange } from '@angular/material/radio';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { BodyDataKey } from '../../enums/body-data-key.enum';
import { RageConnectorService } from 'rage-connector';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'body-mainmenu',
    templateUrl: './body-mainmenu.component.html',
    styleUrls: ['./body-mainmenu.component.scss'],
})
export class BodyMainmenuComponent implements OnInit, OnDestroy {
    bodyMenuNav = BodyMenuNav;
    private _selectedSlot: number;

    @Output() navChanged = new EventEmitter<BodyMenuNav>();

    @Output() saveClicked = new EventEmitter();
    @Output() cancelClicked = new EventEmitter();

    @Input() data: BodyGeneralData[];
    @Output() dataChange = new EventEmitter<BodyGeneralData[]>();

    @Input()
    get selectedSlot() {
        return this._selectedSlot;
    }
    set selectedSlot(value: number) {
        this._selectedSlot = value;
        this.selectedSlotChange.emit(this._selectedSlot);
    }

    @Output() selectedSlotChange = new EventEmitter();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    onGenderChanged(event: MatSelectChange) {
        this.data[this.selectedSlot][0] = event.value == 'true';
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.IsMale, this.data[this.selectedSlot][0]);
    }

    onSlotChanged(event: MatRadioChange) {
        this.selectedSlot = event.value;
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Slot, this.selectedSlot);
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
