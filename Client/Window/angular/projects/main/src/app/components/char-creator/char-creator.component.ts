import { Component, OnInit, ChangeDetectorRef, Input, ChangeDetectionStrategy, NgZone, OnDestroy } from '@angular/core';
import { CharCreatorMenuNav } from './enums/charCreatorMenuNav.enum';
import { SettingsService } from '../../services/settings.service';
import { CharCreateData } from './interfaces/charCreateData';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../enums/dtoserverevent.enum';
import { DToClientEvent } from '../../enums/dtoclientevent.enum';
import { CharCreatorDataKey } from './enums/charCreatorDataKey.enum';

@Component({
    selector: 'app-char-creator',
    templateUrl: './char-creator.component.html',
    styleUrls: ['./char-creator.component.scss'],
    changeDetection: ChangeDetectionStrategy.Default
})
export class CharCreatorComponent implements OnInit, OnDestroy {

    @Input() data: CharCreateData;

    charCreatorMenuNav = CharCreatorMenuNav;

    currentNav = CharCreatorMenuNav.MainMenu;
    /*data: CharCreateData = {
        [0]: [{[0]: true, [99]: 0}, {[0]: true, [99]: 1}, {[0]: true, [99]: 2}, {[0]: true, [99]: 3}],
        [1]: [{
            [0]: 0,
            [1]: 21,
            [2]: 0.5,
            [3]: 0.5, [99]: 0
        },
        {
            [0]: 0,
            [1]: 21,
            [2]: 0.5,
            [3]: 0.5, [99]: 1
        }],
        [2]: [{
            [0]: 0, [1]: 0, [2]: 0, [3]: 0, [4]: 0, [5]: 0, [6]: 0, [7]: 0, [8]: 0, [9]: 0,
            [10]: 0, [11]: 0, [12]: 0, [13]: 0, [14]: 0, [15]: 0, [16]: 0, [17]: 0, [18]: 0, [19]: 0, [99]: 0
        },
        {
            [0]: 0, [1]: 0, [2]: 0, [3]: 0, [4]: 0, [5]: 0, [6]: 0, [7]: 0, [8]: 0, [9]: 0,
            [10]: 0, [11]: 0, [12]: 0, [13]: 0, [14]: 0, [15]: 0, [16]: 0, [17]: 0, [18]: 0, [19]: 0, [99]: 1
        }],
        [3]: [{
            [0]: 0, [1]: 100, [2]: 0, [3]: 100, [4]: 0, [5]: 100, [6]: 0, [7]: 100, [8]: 0, [9]: 100, [10]: 0, [11]: 100,
            [12]: 0, [13]: 100, [14]: 0, [15]: 100, [16]: 0, [17]: 100, [18]: 0, [19]: 100, [20]: 0, [21]: 100, [22]: 0, [23]: 100,
            [24]: 0, [25]: 100, [99]: 0
        },
        {
            [0]: 0, [1]: 100, [2]: 0, [3]: 100, [4]: 0, [5]: 100, [6]: 0, [7]: 100, [8]: 0, [9]: 100, [10]: 0, [11]: 100,
            [12]: 0, [13]: 100, [14]: 0, [15]: 100, [16]: 0, [17]: 100, [18]: 0, [19]: 100, [20]: 0, [21]: 100, [22]: 0, [23]: 100,
            [24]: 0, [25]: 100, [99]: 1
        }],
        [4]: [{
            [0]: 0, [1]: 0, [2]: 0, [3]: 0, [4]: 0, [5]: 0, [6]: 0, [7]: 0, [8]: 0, [99]: 0
        },
        {
            [0]: 0, [1]: 0, [2]: 0, [3]: 0, [4]: 0, [5]: 0, [6]: 0, [7]: 0, [8]: 0, [99]: 1
        }],
        [99]: 0
    };*/

    constructor(private changeDetector: ChangeDetectorRef, public settings: SettingsService,
        private rageConnector: RageConnectorService, private ngZone: NgZone) {

        }

    ngOnInit(): void {
        this.goToMain();
        this.settings.LanguageChanged.on(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.on(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.on(null, this.detectChanges.bind(this));
    }

    ngOnDestroy() {
        this.settings.LanguageChanged.off(null, this.detectChanges.bind(this));
        this.settings.ThemeSettingChangedAfter.off(null, this.detectChanges.bind(this));
        this.settings.SettingsLoaded.off(null, this.detectChanges.bind(this));
    }

    goToMain() {
        this.ngZone.run(() => {
            this.currentNav = CharCreatorMenuNav.MainMenu;
            this.changeDetector.detectChanges();
        });
    }

    goToNav(nav: CharCreatorMenuNav) {
        this.ngZone.run(() => {
            this.currentNav = nav;
            this.changeDetector.detectChanges();
        });
    }

    save() {
        this.rageConnector.callServer(DToServerEvent.SaveCharCreateData, JSON.stringify(this.data));
    }

    cancel() {
        this.rageConnector.callServer(DToServerEvent.CancelCharCreateData);
    }

    recreatePed() {
        this.rageConnector.call(DToClientEvent.CharCreatorDataChanged, CharCreatorDataKey.IsMale, JSON.stringify(this.data));
    }

    setData(list: { 99: number }[], entry: { 99: number }) {
        const index = list.findIndex(e => e[99] == this.data[99]);
        list[index] = entry;
        this.changeDetector.detectChanges();
    }

    getData(list: { 99: number }[]) {
        return list.find(entry => entry[99] == this.data[99]);
    }

    private detectChanges() {
        this.changeDetector.detectChanges();
    }
 }
