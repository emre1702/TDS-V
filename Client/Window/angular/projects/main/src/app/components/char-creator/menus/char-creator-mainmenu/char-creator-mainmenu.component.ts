import { Component, Output, EventEmitter, Input, ChangeDetectorRef, ChangeDetectionStrategy, OnInit, OnDestroy } from '@angular/core';
import { CharCreatorMenuNav } from '../../enums/charCreatorMenuNav.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { MatSelectChange, MatRadioChange } from '@angular/material';
import { CharCreateGeneralData } from '../../interfaces/charCreateGeneralData';
import { RageConnectorService } from 'rage-connector';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'char-creator-mainmenu',
    templateUrl: './char-creator-mainmenu.component.html',
    styleUrls: ['./char-creator-mainmenu.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CharCreatorMainmenuComponent implements OnInit, OnDestroy {

    charCreatorMenuNav = CharCreatorMenuNav;
    private _selectedSlot: number;

    @Output() navChanged = new EventEmitter<CharCreatorMenuNav>();

    @Output() saveClicked = new EventEmitter();
    @Output() cancelClicked = new EventEmitter();

    @Input() data: CharCreateGeneralData[];
    @Output() dataChange = new EventEmitter<CharCreateGeneralData[]>();

    @Output() recreate = new EventEmitter();

    @Input()
    get selectedSlot() {
        return this._selectedSlot;
    }
    set selectedSlot(value: number) {
        this._selectedSlot = value;
        this.selectedSlotChange.emit(this._selectedSlot);
    }

    @Output() selectedSlotChange = new EventEmitter();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }
    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    onGenderChanged(event: MatSelectChange) {
        this.data[this.selectedSlot][0] = event.value == "true";
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

    onSlotChanged(event: MatRadioChange) {
        this.selectedSlot = event.value;
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

}
