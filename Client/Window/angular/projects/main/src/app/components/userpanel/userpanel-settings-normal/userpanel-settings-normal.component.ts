import { UserpanelSettingsNormalService } from './services/userpanel-settings-normal.service';
import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { UserpanelSettingsNormalType } from './enums/userpanel-settings-normal-type.enum';
import { Subscription } from 'rxjs';
import { SettingsService } from '../../../services/settings.service';

@Component({
    selector: 'app-userpanel-settings-normal',
    templateUrl: './userpanel-settings-normal.component.html',
    styleUrls: ['./userpanel-settings-normal.component.scss']
})
export class UserpanelSettingsNormalComponent implements OnInit, OnDestroy {

    currentNav?: UserpanelSettingsNormalType;

    settingNormalType = UserpanelSettingsNormalType;

    private subscriptions: Subscription[] = [];

    constructor(
        public settings: SettingsService,
        public userpanelSettingsNormalService: UserpanelSettingsNormalService,
        public changeDetector: ChangeDetectorRef) { }

    ngOnInit() {
        this.subscriptions.push(this.userpanelSettingsNormalService.settingsLoaded.subscribe(this.detectChanges.bind(this)));
    }

    ngOnDestroy() {
        for (const subscription of this.subscriptions) {
            subscription.unsubscribe();
        }
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
