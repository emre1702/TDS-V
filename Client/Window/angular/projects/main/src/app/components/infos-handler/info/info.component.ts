import { Component, OnInit, ChangeDetectionStrategy, Input, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-info',
    templateUrl: './info.component.html',
    styleUrls: ['./info.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class InfoComponent implements OnInit, OnDestroy {

    @Input() messageKey: string;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef) { }

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }
}
