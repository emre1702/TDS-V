import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { LanguageEnum } from 'projects/main/src/app/enums/language.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';

@Component({
    selector: 'app-map-creator-description',
    templateUrl: './map-creator-description.component.html',
    styleUrls: ['./map-creator-description.component.scss'],
})
export class MapCreatorDescriptionComponent {
    @Input() formGroup: FormGroup;

    languageEnum = LanguageEnum;

    constructor(public settings: SettingsService) {}

    getLanguages(): string[] {
        const keys = Object.keys(LanguageEnum);
        return keys.slice(keys.length / 2);
    }
}
