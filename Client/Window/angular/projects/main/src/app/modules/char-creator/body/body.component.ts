import { Component, OnInit, ChangeDetectorRef, Input, NgZone, OnDestroy, EventEmitter, Output } from '@angular/core';
import { BodyMenuNav } from './enums/body-menu-nav.enum';
import { SettingsService } from '../../../services/settings.service';
import { BodyData } from './models/body-data';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { BodyDataKey } from './enums/body-data-key.enum';

@Component({
    selector: 'app-body',
    templateUrl: './body.component.html',
    styleUrls: ['./body.component.scss'],
})
export class BodyComponent implements OnInit, OnDestroy {
    @Input() data: BodyData;
    @Output() back = new EventEmitter();

    bodyMenuNav = BodyMenuNav;

    currentNav = BodyMenuNav.MainMenu;

    constructor(private changeDetector: ChangeDetectorRef, public settings: SettingsService, private rageConnector: RageConnectorService) {}

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    goBack() {
        if (this.currentNav === BodyMenuNav.MainMenu) {
            this.back.emit();
            return;
        }
        this.currentNav = BodyMenuNav.MainMenu;
        this.changeDetector.detectChanges();
    }

    goToNav(nav: BodyMenuNav) {
        this.currentNav = nav;
        this.changeDetector.detectChanges();
    }

    save() {
        this.rageConnector.callServer(ToServerEvent.SaveCharCreateData, JSON.stringify(this.data));
    }

    cancel() {
        this.rageConnector.callServer(ToServerEvent.CancelCharCreateData);
    }

    recreatePed() {
        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.All, JSON.stringify(this.data));
    }

    setData(list: { 99: number }[], entry: { 99: number }) {
        const index = list.findIndex((e) => e[99] == this.data[99]);
        list[index] = entry;
        this.changeDetector.detectChanges();
    }

    getData(list: { 99: number }[]) {
        return list.find((entry) => entry[99] == this.data[99]);
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
