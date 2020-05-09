import { Component, Output, EventEmitter, Input, ChangeDetectorRef, ChangeDetectionStrategy, OnInit, OnDestroy } from '@angular/core';
import { CharCreatorMenuNav } from '../../enums/charCreatorMenuNav.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { MatSelectChange } from '@angular/material';
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

    @Output() navChanged = new EventEmitter<CharCreatorMenuNav>();

    @Output() saveClicked = new EventEmitter();
    @Output() cancelClicked = new EventEmitter();

    @Input() data: CharCreateGeneralData;
    @Output() dataChange = new EventEmitter<CharCreateGeneralData>();

    @Output() recreate = new EventEmitter();

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }
    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    onGenderChanged(event: MatSelectChange) {
        this.data[0] = event.value == "true";
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

}
