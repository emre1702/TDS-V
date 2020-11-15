import { Input, Component } from '@angular/core';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { ButtonRow } from '../../models/button-row';

@Component({
    selector: 'app-button-row',
    templateUrl: './button-row.component.html',
    styleUrls: ['./button-row.component.scss']
})
export class ButtonRowComponent {
    @Input() setting: ButtonRow;

    constructor(public settingsService: SettingsService) {}
}
