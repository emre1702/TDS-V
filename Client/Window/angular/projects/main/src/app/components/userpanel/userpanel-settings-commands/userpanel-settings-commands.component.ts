import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { RageConnectorService } from 'rage-connector';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelService } from '../services/userpanel.service';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { UserpanelSettingCommandConfiguredDataDto } from '../interfaces/settings-commands/userpanelSettingCommandConfiguredDataDto';

@Component({
    selector: 'app-userpanel-settings-commands',
    templateUrl: './userpanel-settings-commands.component.html',
    styleUrls: ['./userpanel-settings-commands.component.scss'],
})
export class UserpanelSettingsCommandsComponent implements OnInit, OnDestroy {
    constructor(
        private rageConnector: RageConnectorService,
        public settings: SettingsService,
        public userpanelService: UserpanelService,
        private changeDetector: ChangeDetectorRef
    ) {}

    ngOnInit() {
        this.userpanelService.settingsCommandsDataLoaded.on(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.userpanelService.settingsCommandsDataLoaded.off(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));

        this.save();
    }

    addNewRow() {
        const entry = { 0: 0, 1: 0, 2: '', initial: false, changed: false };
        this.userpanelService.settingsCommandsData[1].push(entry);
        this.changeDetector.detectChanges();
    }

    changed(entry: UserpanelSettingCommandConfiguredDataDto) {
        entry.changed = true;
    }

    delete(entry: UserpanelSettingCommandConfiguredDataDto) {
        if (!!entry[0]) {
            entry[2] = undefined;
            entry.changed = true;
        } else {
            const index = this.userpanelService.settingsCommandsData[1].indexOf(entry);
            if (index >= 0) {
                this.userpanelService.settingsCommandsData[1].splice(index, 1);
            }
        }
        this.changeDetector.detectChanges();
    }

    save() {
        const entries = this.userpanelService.settingsCommandsData[1].filter((d) => d.changed && d[1] != 0);
        if (entries.length === 0) {
            return;
        }
        for (const entry of entries) {
            entry.changed = undefined;

            if (!entry[2]) entry[2] = '';
            entry[2].replace(' ', '');
        }

        this.rageConnector.callServer(ToServerEvent.SavePlayerCommandsSettings, JSON.stringify(entries));
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
