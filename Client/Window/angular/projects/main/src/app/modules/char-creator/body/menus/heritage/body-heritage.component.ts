import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { BodyHeritageData } from '../../models/body-heritage-data';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { BodyDataKey } from '../../enums/body-data-key.enum';
import { MatSelectChange } from '@angular/material/select';
import { MatSliderChange } from '@angular/material/slider';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'body-heritage',
    templateUrl: './body-heritage.component.html',
    styleUrls: ['./body-heritage.component.scss'],
})
export class BodyHeritageComponent implements OnInit, OnDestroy {
    @Input() data: BodyHeritageData;
    @Output() dataChange = new EventEmitter<BodyHeritageData>();

    @Output() randomized = new EventEmitter();

    fathers: { Id: number; Name: string }[] = [
        { Id: 0, Name: 'Benjamin' },
        { Id: 1, Name: 'Daniel' },
        { Id: 2, Name: 'Joshua' },
        { Id: 3, Name: 'Noah' },
        { Id: 4, Name: 'Andrew' },
        { Id: 5, Name: 'Juan' },
        { Id: 6, Name: 'Alex' },
        { Id: 7, Name: 'Isaac' },
        { Id: 8, Name: 'Evan' },
        { Id: 9, Name: 'Ethan' },
        { Id: 10, Name: 'Vincent' },
        { Id: 11, Name: 'Angel' },
        { Id: 12, Name: 'Diego' },
        { Id: 13, Name: 'Adrian' },
        { Id: 14, Name: 'Gabriel' },
        { Id: 15, Name: 'Michael' },
        { Id: 16, Name: 'Santiago' },
        { Id: 17, Name: 'Kevin' },
        { Id: 18, Name: 'Louis' },
        { Id: 19, Name: 'Samuel' },
        { Id: 20, Name: 'Anthony' },
        { Id: 42, Name: 'Claude' },
        { Id: 43, Name: 'Niko' },
        { Id: 44, Name: 'John' },
    ];
    mothers: { Id: number; Name: string }[] = [
        { Id: 21, Name: 'Hannah' },
        { Id: 22, Name: 'Aubrey' },
        { Id: 23, Name: 'Jasmine' },
        { Id: 24, Name: 'Gisele' },
        { Id: 25, Name: 'Amelia' },
        { Id: 26, Name: 'Isabella' },
        { Id: 27, Name: 'Zoe' },
        { Id: 28, Name: 'Ava' },
        { Id: 29, Name: 'Camila' },
        { Id: 30, Name: 'Violet' },
        { Id: 31, Name: 'Sophia' },
        { Id: 32, Name: 'Evelyn' },
        { Id: 33, Name: 'Nicole' },
        { Id: 34, Name: 'Ashley' },
        { Id: 35, Name: 'Gracie' },
        { Id: 36, Name: 'Brianna' },
        { Id: 37, Name: 'Natalie' },
        { Id: 38, Name: 'Olivia' },
        { Id: 39, Name: 'Elizabeth' },
        { Id: 40, Name: 'Charlotte' },
        { Id: 41, Name: 'Emma' },
        { Id: 45, Name: 'Misty' },
    ];

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    onFatherChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Heritage, JSON.stringify(this.data));
    }

    onMotherChanged(event: MatSelectChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Heritage, JSON.stringify(this.data));
    }

    onResemblanceChanged(event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Heritage, JSON.stringify(this.data));
    }

    onSkinToneChanged(event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Heritage, JSON.stringify(this.data));
    }

    randomize() {
        this.data[0] = this.fathers[Math.floor(Math.random() * this.fathers.length)].Id;
        this.data[1] = this.mothers[Math.floor(Math.random() * this.mothers.length)].Id;
        this.data[2] = Math.floor(Math.random() * (100 + 1)) / 100;
        this.data[3] = Math.floor(Math.random() * (100 + 1)) / 100;

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.randomized.emit();
    }

    getPercentage(value: number) {
        return Math.round(value * 100) + '%';
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
