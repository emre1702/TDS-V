import { Component, OnInit, ChangeDetectorRef, Output, Input, ChangeDetectionStrategy, EventEmitter, ViewChild } from '@angular/core';
import { CustomLobbyWeaponData } from '../../models/custom-lobby-weapon-data';
import { WeaponHashGroupConstants } from '../../enums/weapon-hash-group.constants';
import { WeaponHash } from '../../enums/weapon-hash.enum';
import { WeaponType } from '../../enums/weapon-type.enum';
import { SettingsService } from 'projects/main/src/app/services/settings.service';
import { MatTableDataSource, MatSort, MatSnackBar } from '@angular/material';
import { CustomLobbyArmsRaceWeaponData } from '../../models/custom-lobby-armsraceweapon-data';

@Component({
    selector: 'app-custom-lobby-armsraceweapons-menu',
    templateUrl: './custom-lobby-armsraceweapons-menu.component.html',
    styleUrls: ['./custom-lobby-armsraceweapons-menu.component.scss'],
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomLobbyArmsRaceWeaponsMenuComponent implements OnInit {

    @Input() creating: boolean;
    @Input() selectedWeapons: CustomLobbyArmsRaceWeaponData[];
    @Input() allWeapons: CustomLobbyWeaponData[];
    @Output() backClicked = new EventEmitter<CustomLobbyArmsRaceWeaponData[]>();

    @ViewChild("selectedWeaponsSort", { static: true }) selectedWeaponsSort: MatSort;

    selectedWeaponsDataSource: MatTableDataSource<CustomLobbyArmsRaceWeaponData>;
    weaponHash = WeaponHash;
    weaponHashGroups: [string, [string, WeaponHash, boolean][]][] = [];
    selectedWeaponsTableColumns = ['Name', 'AtKill', 'Delete'];

    constructor(
        private changeDetector: ChangeDetectorRef,
        public settings: SettingsService,
        private snackBar: MatSnackBar
    ) { }

    ngOnInit() {
        for (const weaponType in WeaponHashGroupConstants.data) {
            let arr: [string, WeaponHash, boolean][] = [];
            for (const weaponName of WeaponHashGroupConstants.data[weaponType]) {
                const weaponHash = WeaponHash[weaponName as string] as WeaponHash;
                if (!weaponHash) {
                    continue;
                }
                const weapon = this.allWeapons.find(w => w[0] == weaponHash);
                if (!weapon) {
                    continue;
                }

                const selectedWeapon = this.selectedWeapons.find(w => w[0] == weaponHash);
                let selected = false;
                if (selectedWeapon) {
                    selected = true;

                    if (!selectedWeapon[0] && selectedWeapon[0] != 0) {
                        selectedWeapon[0] = weapon[0];
                    }
                    if (!selectedWeapon[1] && selectedWeapon[1] != 0) {
                        selectedWeapon[1] = weapon[1];
                    }
                }

                arr.push([weaponName, weaponHash, selected]);
            }
            arr = arr.sort((a, b) => a[0].localeCompare(b[0]));
            this.weaponHashGroups.push([WeaponType[weaponType as unknown as number], arr]);
        }

        this.selectedWeaponsDataSource = new MatTableDataSource(this.selectedWeapons);
        this.selectedWeaponsDataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
        this.selectedWeaponsDataSource.sort = this.selectedWeaponsSort;

        this.changeDetector.detectChanges();
    }

    backButtonClicked() {
        if (!this.fixData()) {
            this.selectedWeaponsDataSource.data = this.selectedWeapons;
            this.changeDetector.detectChanges();
            return;
        }

        this.backClicked.emit(this.selectedWeapons);
    }

    clearButtonClicked() {
        for (const group of this.weaponHashGroups) {
            for (const weapon of group[1]) {
                weapon[2] = false;
            }
        }
        this.selectedWeapons = [];
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        this.changeDetector.detectChanges();
    }

    selectWeapon(weaponData: [string, WeaponHash, boolean]) {
        if (weaponData[2]) {
            return;
        }

        weaponData[2] = true;
        this.selectedWeapons.push({ 0: weaponData[1], 1: this.getMaxSelectedAtKill() + 1 });
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        this.changeDetector.detectChanges();
    }

    unselectWeapon(weapon: CustomLobbyWeaponData) {
        const index = this.selectedWeapons.indexOf(weapon);
        this.selectedWeapons.splice(index, 1);
        this.selectedWeaponsDataSource.data = this.selectedWeapons;

        try {
            const weaponGroup = this.weaponHashGroups.find(g => g[1].find(w => w[1] == weapon[0]));
            const weaponData = weaponGroup[1].find(w => w[1] == weapon[0]);

            weaponData[2] = false;
        } catch {}

        this.changeDetector.detectChanges();
    }

    sortingDataAccessor(obj: CustomLobbyWeaponData, property: any) {
        const index = this.selectedWeaponsTableColumns.indexOf(property);
        if (index == 0) {
            return WeaponHash[obj[index]];
        }
        return obj[index];
    }

    private fixData(): boolean {
        const firstWeapon = this.selectedWeapons.find(w => w[1] == 0);
        if (!firstWeapon) {
            this.snackBar.open(this.settings.Lang.ArmsRaceWeaponsFirstWeaponError, "OK",
                { duration: undefined, panelClass: ["mat-app-background", "snackbar"] });
            return false;
        }

        const seen = {};
        const hasDuplicates = this.selectedWeapons.some(e => {
            if (seen.hasOwnProperty(e[1])) {
                return true;
            }

            return (seen[e[1]] = false);
        });

        if (hasDuplicates) {
            this.snackBar.open(this.settings.Lang.ArmsRaceWeaponsDuplicateError, "OK",
                { duration: undefined, panelClass: ["mat-app-background", "snackbar"] });
            return false;
        }

        const maxSelectedAtKill = this.getMaxSelectedAtKill();

        const endEntry = this.selectedWeapons.find(w => !w[0]);
        if (!endEntry) {
            this.selectedWeapons.push({ 0: null, 1: maxSelectedAtKill + 1 });
            this.snackBar.open(this.settings.Lang.ArmsRaceWeaponsWinError, "OK",
                { duration: undefined, panelClass: ["mat-app-background", "snackbar"] });
            return false;
        }
        if (endEntry[1] != maxSelectedAtKill) {
            endEntry[1] = maxSelectedAtKill + 1;
            this.snackBar.open(this.settings.Lang.ArmsRaceWeaponsWinNotLastError, "OK",
                { duration: undefined, panelClass: ["mat-app-background", "snackbar"] });
            return false;
        }

        if (this.selectedWeapons) {
            for (const weapon of this.selectedWeapons) {
                weapon[1] = Math.floor(weapon[1]);  // Only allow integer for ammo
            }
        }


        return true;
    }

    private getMaxSelectedAtKill() {
        let max = -1;
        for (const weapon of this.selectedWeapons) {
            if (weapon[1] > max)
                max = weapon[1];
        }
        return max;
    }
}
