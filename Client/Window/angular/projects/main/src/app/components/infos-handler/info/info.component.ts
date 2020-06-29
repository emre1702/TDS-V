import { Component, OnInit, ChangeDetectionStrategy, Input } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-info',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfoComponent {

    @Input() messageKey: string;

    constructor(public settings: SettingsService) { }
}
