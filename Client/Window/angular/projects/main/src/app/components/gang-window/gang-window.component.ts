import { Component, OnInit, ChangeDetectorRef, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { GangWindowNav } from './enums/gang-window-nav.enum';
import { GangWindowService } from './services/gang-window-service';

@Component({
    selector: 'app-gang-window',
    templateUrl: './gang-window.component.html',
    styleUrls: ['./gang-window.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush,
    providers: [GangWindowService]
})
export class GangWindowComponent implements OnInit, OnDestroy {

    currentNav: GangWindowNav;

    gangWindowNav = GangWindowNav;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef,
        private gangWindowService: GangWindowService) { }

    ngOnInit(): void {
        this.currentNav = this.settings.IsInGang ? GangWindowNav.GangInfo : GangWindowNav.Create;

        this.settings.IsInGangChanged.on(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.IsInGangChanged.off(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    closeFunc() {

    }

    gotoNav(nav: GangWindowNav) {
        this.currentNav = nav;
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
