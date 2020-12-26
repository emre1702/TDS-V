import { Component, OnInit, ChangeDetectorRef, EventEmitter, Input, Output, OnDestroy } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { BodyFeaturesData } from '../../models/body-features-data';
import { RageConnectorService } from 'rage-connector';
import { ToClientEvent } from 'projects/main/src/app/enums/to-client-event.enum';
import { BodyDataKey } from '../../enums/body-data-key.enum';
import { MatSliderChange } from '@angular/material/slider';

@Component({
    // tslint:disable-next-line: component-selector
    selector: 'body-features',
    templateUrl: './body-features.component.html',
    styleUrls: ['./body-features.component.scss'],
})
export class BodyFeaturesComponent implements OnInit, OnDestroy {
    @Input() data: BodyFeaturesData;
    @Output() dataChange = new EventEmitter<BodyFeaturesData>();

    @Output() randomized = new EventEmitter();

    features: { Name: string; MinDescription: string; MaxDescription: string }[] = [
        { Name: 'Nose Width', MinDescription: 'narrow', MaxDescription: 'wide' },
        { Name: 'Nose Bottom Height', MinDescription: 'top', MaxDescription: 'bottom' },
        { Name: 'Nose Tip Length', MinDescription: 'grand', MaxDescription: 'petite' },
        { Name: 'Nose Bridge Depth', MinDescription: 'round', MaxDescription: 'hollow' },
        { Name: 'Nose Tip Height', MinDescription: 'upward', MaxDescription: 'downward' },
        { Name: 'Nose Broken', MinDescription: 'to right', MaxDescription: 'to left' },
        { Name: 'Brow Height', MinDescription: 'top', MaxDescription: 'bottom' },
        { Name: 'Brow Depth', MinDescription: 'inward', MaxDescription: 'outward' },
        { Name: 'Cheekbone Height', MinDescription: 'top', MaxDescription: 'bottom' },
        { Name: 'Cheekbone Width', MinDescription: 'narrow', MaxDescription: 'wide' },
        { Name: 'Cheek Depth', MinDescription: 'wide', MaxDescription: 'narrow' },
        { Name: 'Eye Size', MinDescription: 'opened', MaxDescription: 'closed' },
        { Name: 'Lip Thickness', MinDescription: 'wide', MaxDescription: 'narrow' },
        { Name: 'Jaw Width', MinDescription: 'narrow', MaxDescription: 'wide' },
        { Name: 'Jaw Height', MinDescription: 'top', MaxDescription: 'bottom' },
        { Name: 'Chin Height', MinDescription: 'small', MaxDescription: 'long' },
        { Name: 'Chin Depth', MinDescription: 'inward', MaxDescription: 'outward' },
        { Name: 'Chin Width', MinDescription: 'narrow', MaxDescription: 'grand' },
        { Name: 'Chin Shape', MinDescription: 'simple chin', MaxDescription: 'double chin' },
        { Name: 'Neck Width', MinDescription: 'narrow', MaxDescription: 'wide' },
    ];

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef, private rageConnector: RageConnectorService) {}

    ngOnInit() {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
    }

    onFeatureChanged(index: number, event: MatSliderChange) {
        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.Feature, index, this.data[index]);
    }

    randomize() {
        for (let i = 0; i < this.features.length; ++i) {
            this.data[i] = this.getRandom();
        }

        this.dataChange.emit(this.data);
        this.changeDetector.detectChanges();

        this.randomized.emit();
    }

    private getRandom() {
        return Math.round(Math.floor(Math.random() * (100 + 100 + 1)) - 100) / 100;
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
