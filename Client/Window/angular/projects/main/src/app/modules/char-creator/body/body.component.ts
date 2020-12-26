import { Component, OnInit, ChangeDetectorRef, OnDestroy, EventEmitter, Output } from '@angular/core';
import { BodyMenuNav } from './enums/body-menu-nav.enum';
import { SettingsService } from '../../../services/settings.service';
import { RageConnectorService } from 'rage-connector';
import { ToServerEvent } from '../../../enums/to-server-event.enum';
import { ToClientEvent } from '../../../enums/to-client-event.enum';
import { BodyDataKey } from './enums/body-data-key.enum';
import { BodyService } from './services/body.service';
import { tap } from 'rxjs/operators';
import { BodyData } from './models/body-data';

@Component({
    selector: 'app-body',
    templateUrl: './body.component.html',
    styleUrls: ['./body.component.scss'],
})
export class BodyComponent implements OnInit, OnDestroy {
    data$ = this.service.getData().pipe(tap((data) => (this._data = data)));
    private _data: BodyData;

    @Output() back = new EventEmitter();

    bodyMenuNav = BodyMenuNav;

    currentNav = BodyMenuNav.MainMenu;

    constructor(
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService,
        private rageConnector: RageConnectorService,
        private service: BodyService
    ) {}

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
        this.rageConnector.callServer(ToServerEvent.SaveBodyData, JSON.stringify(this._data));
    }

    recreatePed() {
        this.rageConnector.call(ToClientEvent.BodyDataChanged, BodyDataKey.All, JSON.stringify(this._data));
    }

    setData(list: { 99: number }[], entry: { 99: number }) {
        const index = list.findIndex((e) => e[99] == this._data[99]);
        list[index] = entry;
        this.changeDetector.detectChanges();
    }

    getData(list: { 99: number }[]) {
        return list.find((entry) => entry[99] == this._data[99]);
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
