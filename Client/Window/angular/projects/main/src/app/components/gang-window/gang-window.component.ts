import { Component, OnInit, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../services/settings.service';
import { GangWindowNav } from './enums/gang-window-nav.enum';
import { GangWindowService } from './services/gang-window-service';
import { GangCommand } from './enums/gang-command.enum';
import { GangWindowOnlyOneEditorPage } from './enums/gang-window-only-one-editor-page.enum';
import { RageConnectorService } from 'rage-connector';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';

@Component({
    selector: 'app-gang-window',
    templateUrl: './gang-window.component.html',
    styleUrls: ['./gang-window.component.scss'],
    providers: [GangWindowService]
})
export class GangWindowComponent implements OnInit, OnDestroy {

    currentNav: GangWindowNav;

    gangWindowNav = GangWindowNav;

    constructor(public settings: SettingsService, private changeDetector: ChangeDetectorRef,
        public gangWindowService: GangWindowService, private rageConnector: RageConnectorService) { }

    ngOnInit(): void {
        this.settings.IsInGangChanged.on(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChanged.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
        this.gangWindowService.loadingDataChanged.on(null, this.detectChanges.bind(this));

        this.gotoNav(GangWindowNav.MainMenu);
    }

    ngOnDestroy(): void {
        this.settings.IsInGangChanged.off(null, this.detectChanges.bind(this));
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChanged.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
        this.gangWindowService.loadingDataChanged.off(null, this.detectChanges.bind(this));

        this.checkLeavePage();
    }

    closeFunc() {
        this.rageConnector.call(DToClientEvent.CloseGangWindow);
    }

    gotoNav(nav: GangWindowNav) {
        switch (nav) {
            case GangWindowNav.RanksLevels:
                this.gangWindowService.executeCommand(GangCommand.OpenOnlyOneEditorPage, [GangWindowOnlyOneEditorPage.RanksLevels],
                    this.gotoNavFinal.bind(this, nav), false, false);
                break;
            case GangWindowNav.RanksPermissions:
                this.gangWindowService.executeCommand(GangCommand.OpenOnlyOneEditorPage, [GangWindowOnlyOneEditorPage.RanksPermissions],
                    this.gotoNavFinal.bind(this, nav), false, false);
                break;
            default:
                this.gotoNavFinal(nav);
                break;
        }
    }

    private checkLeavePage() {
        switch (this.currentNav) {
            case GangWindowNav.RanksLevels:
                this.gangWindowService.executeCommand(GangCommand.CloseOnlyOneEditorPage, [GangWindowOnlyOneEditorPage.RanksLevels],
                    () => {}, false, false);
                break;
            case GangWindowNav.RanksPermissions:
                this.gangWindowService.executeCommand(GangCommand.CloseOnlyOneEditorPage, [GangWindowOnlyOneEditorPage.RanksPermissions],
                    () => {}, false, false);
                break;
        }
    }

    private gotoNavFinal(nav: GangWindowNav) {
        this.checkLeavePage();

        this.currentNav = nav;
        this.gangWindowService.loadData(nav);
        this.changeDetector.detectChanges();
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
}
