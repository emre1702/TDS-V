import { Component, OnInit, ChangeDetectionStrategy, Input, ChangeDetectorRef, OnDestroy } from '@angular/core';
import { SettingsService } from '../../../services/settings.service';
import { UserpanelPlayerWeaponStatsData } from '../interfaces/stats/userpanelPlayerWeaponStatsData';
import { WeaponHash } from '../../lobbychoice/enums/weapon-hash.enum';
import { UserpanelWeaponStats } from '../enums/userpanel-weapon-stats.enum';
import { UserpanelWeaponBodypartStats } from '../enums/userpanel-weapon-bodypart-stats.enum';
import { PedBodyPart } from '../enums/ped-body-part.enum';
import { RageConnectorService } from 'rage-connector';
import { DToServerEvent } from '../../../enums/dtoserverevent.enum';

@Component({
    selector: 'app-userpanel-stats-weapon',
    templateUrl: './userpanel-stats-weapon.component.html',
    styleUrls: ['./userpanel-stats-weapon.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class UserpanelStatsWeaponComponent implements OnInit, OnDestroy {

    @Input()
    set weaponsUsed(value: string[]) {
        this._weaponsUsed = value;
        this.changeDetector.detectChanges();
    }
    get weaponsUsed(): string[] {
        return this._weaponsUsed;
    }

    weaponStatsData: { [key: string]: UserpanelPlayerWeaponStatsData } = {};
    UserpanelWeaponStats = UserpanelWeaponStats;
    UserpanelWeaponBodypartStats = UserpanelWeaponBodypartStats;
    PedBodyPart = PedBodyPart;

    private _weaponsUsed: string[];

    constructor(
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService,
        private rageConnector: RageConnectorService) { }

    ngOnInit(): void {
        this.settings.LanguageChanged.on(null, this.changeDetector.detectChanges.bind(this));
    }

    ngOnDestroy(): void {
        this.settings.LanguageChanged.off(null, this.changeDetector.detectChanges.bind(this));
    }

    loadWeaponData(weaponName: string) {
        if (!this.weaponStatsData[weaponName]) {
            this.rageConnector.callCallbackServer(DToServerEvent.LoadPlayerWeaponStats, [weaponName], (json: string) => {
                this.weaponStatsData[weaponName] = JSON.parse(json);
                this.changeDetector.detectChanges();
            });
        }

        /*this.weaponStatsData[weaponName] = {
            0: weaponName,
            1: [
                { 0: PedBodyPart.Head, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Neck, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.UpperBody, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Spine, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.LowerBody, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Arm, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Hand, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Leg, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 },
                { 0: PedBodyPart.Foot, 1: 231, 2: 1231, 3: 12321, 4: 123321, 5: 131, 6: 123 }
            ],
            2: 123,
            3: 123,
            4: 12312,
            5: 312,
            6: 13,
            7: 1234,
            8: 123,
            9: 111,
            10: 3555,
            11: 352
        };
        this.changeDetector.detectChanges();*/
    }

    getWeaponStatKeys(): Array<string> {
        const keys = Object.keys(UserpanelWeaponStats);
        console.log(keys);
        console.log(keys.slice(keys.length / 2));
        return keys.slice(keys.length / 2);
    }

    getWeaponBodypartStatKeys(): Array<string> {
        const keys = Object.keys(UserpanelWeaponBodypartStats);
        return keys.slice(keys.length / 2);
    }
}
