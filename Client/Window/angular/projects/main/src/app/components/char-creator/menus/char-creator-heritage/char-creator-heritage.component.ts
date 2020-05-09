import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, ChangeDetectionStrategy, AfterViewInit, OnDestroy } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { CharCreateHeritageData } from '../../interfaces/charCreateHeritageData';
import { MatSelectChange, MatSliderChange } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from 'projects/main/src/app/enums/dtoclientevent.enum';
import { CharCreatorDataKey } from '../../enums/charCreatorDataKey.enum';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'char-creator-heritage',
    templateUrl: './char-creator-heritage.component.html',
    styleUrls: ['./char-creator-heritage.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default
})
export class CharCreatorHeritageComponent implements OnInit, OnDestroy {

    @Input() data: CharCreateHeritageData;
    @Output() dataChange = new EventEmitter<CharCreateHeritageData>();

    @Output() recreate = new EventEmitter();

    fathers: { Id: number, Name: string }[] = [
        { Id: 0, Name: "Benjamin" }, { Id: 1, Name: "Daniel" },
        { Id: 2, Name: "Joshua" }, { Id: 3, Name: "Noah" },
        { Id: 4, Name: "Andrew" }, { Id: 5, Name: "Juan" },
        { Id: 6, Name: "Alex" }, { Id: 7, Name: "Isaac" },
        { Id: 8, Name: "Evan" }, { Id: 9, Name: "Ethan" },
        { Id: 10, Name: "Vincent" }, { Id: 11, Name: "Angel" },
        { Id: 12, Name: "Diego" }, { Id: 13, Name: "Adrian" },
        { Id: 14, Name: "Gabriel" }, { Id: 15, Name: "Michael" },
        { Id: 16, Name: "Santiago" }, { Id: 17, Name: "Kevin" },
        { Id: 18, Name: "Louis" }, { Id: 19, Name: "Samuel" },
        { Id: 20, Name: "Anthony" }, { Id: 42, Name: "Claude" },
        { Id: 43, Name: "Niko" }, { Id: 44, Name: "John" }
    ];
    mothers: { Id: number, Name: string }[] = [
        { Id: 21, Name: "Hannah" }, { Id: 22, Name: "Aubrey" },
        { Id: 23, Name: "Jasmine" }, { Id: 24, Name: "Gisele" },
        { Id: 25, Name: "Amelia" }, { Id: 26, Name: "Isabella" },
        { Id: 27, Name: "Zoe" }, { Id: 28, Name: "Ava" },
        { Id: 29, Name: "Camila" }, { Id: 30, Name: "Violet" },
        { Id: 31, Name: "Sophia" }, { Id: 32, Name: "Evelyn" },
        { Id: 33, Name: "Nicole" }, { Id: 34, Name: "Ashley" },
        { Id: 35, Name: "Gracie" }, { Id: 36, Name: "Brianna" },
        { Id: 37, Name: "Natalie" }, { Id: 38, Name: "Olivia" },
        { Id: 39, Name: "Elizabeth" }, { Id: 40, Name: "Charlotte" },
        { Id: 41, Name: "Emma" }, { Id: 45, Name: "Misty" }
    ];

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    onFatherChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Heritage, JSON.stringify(this.data));
    }

    onMotherChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Heritage, JSON.stringify(this.data));
    }

    onResemblanceChanged(event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Heritage, JSON.stringify(this.data));
    }

    onSkinToneChanged(event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Heritage, JSON.stringify(this.data));
    }

    randomize() {
        this.data[0] = this.fathers[Math.floor(Math.random() * this.fathers.length)].Id;
        this.data[1] = this.mothers[Math.floor(Math.random() * this.mothers.length)].Id;
        this.data[2] = (Math.floor(Math.random() * (100 + 1)) / 100);
        this.data[3] = (Math.floor(Math.random() * (100 + 1)) / 100);

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

    getPercentage(value: number) { return Math.round(value * 100) + "%"; }
}
