import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { MapCreateDataKey } from '../../enums/map-create-data-key';

@Component({
    selector: 'app-map-creator-settings',
    templateUrl: './map-creator-settings.component.html',
    styleUrls: ['./map-creator-settings.component.scss'],
})
export class MapCreatorSettingsComponent {
    @Input() formGroup: FormGroup;

    constructor(public settings: SettingsService) {}
}
