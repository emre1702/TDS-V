import { Component, OnInit, ChangeDetectorRef, EventEmitter, Input, Output, ChangeDetectionStrategy, OnDestroy } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { CharCreateFeaturesData } from '../../interfaces/charCreateFeaturesData';
import { MatSliderChange } from '@angular/material';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from 'projects/main/src/app/enums/dtoclientevent.enum';
import { CharCreatorDataKey } from '../../enums/charCreatorDataKey.enum';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'char-creator-features',
    templateUrl: './char-creator-features.component.html',
    styleUrls: ['./char-creator-features.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default
})
export class CharCreatorFeaturesComponent implements OnInit, OnDestroy {

    @Input() data: CharCreateFeaturesData;
    @Output() dataChange = new EventEmitter<CharCreateFeaturesData>();

    @Output() recreate = new EventEmitter();

    features: { Name: string, MinDescription: string, MaxDescription: string }[] = [
        { Name: "Nose Width", MinDescription: "narrow", MaxDescription: "wide" },
        { Name: "Nose Bottom Height", MinDescription: "top", MaxDescription: "bottom" },
        { Name: "Nose Tip Length", MinDescription: "grand", MaxDescription: "petite" },
        { Name: "Nose Bridge Depth", MinDescription: "round", MaxDescription: "hollow" },
        { Name: "Nose Tip Height", MinDescription: "upward", MaxDescription: "downward" },
        { Name: "Nose Broken", MinDescription: "to right", MaxDescription: "to left" },
        { Name: "Brow Height", MinDescription: "top", MaxDescription: "bottom" },
        { Name: "Brow Depth", MinDescription: "inward", MaxDescription: "outward" },
        { Name: "Cheekbone Height", MinDescription: "top", MaxDescription: "bottom" },
        { Name: "Cheekbone Width", MinDescription: "narrow", MaxDescription: "wide" },
        { Name: "Cheek Depth", MinDescription: "wide", MaxDescription: "narrow" },
        { Name: "Eye Size", MinDescription: "opened", MaxDescription: "closed" },
        { Name: "Lip Thickness", MinDescription: "wide", MaxDescription: "narrow" },
        { Name: "Jaw Width", MinDescription: "narrow", MaxDescription: "wide" },
        { Name: "Jaw Height", MinDescription: "top", MaxDescription: "bottom" },
        { Name: "Chin Height", MinDescription: "small", MaxDescription: "long" },
        { Name: "Chin Depth", MinDescription: "inward", MaxDescription: "outward" },
        { Name: "Chin Width", MinDescription: "narrow", MaxDescription: "grand" },
        { Name: "Chin Shape", MinDescription: "simple chin", MaxDescription: "double chin" },
        { Name: "Neck Width", MinDescription: "narrow", MaxDescription: "wide" }
    ];

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef,
        private rageConnector: RageConnectorService) { }


    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    onFeatureChanged(index: number, event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.Feature, index, this.data[index]);
    }

    randomize() {
        for (let i = 0; i < this.features.length; ++i) {
            this.data[i] = this.getRandom();
        }

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.recreate.emit();
    }

    private getRandom() {
        return Math.round(Math.floor(Math.random() * (100 + 100 + 1)) - 100) / 100;
    }
}
